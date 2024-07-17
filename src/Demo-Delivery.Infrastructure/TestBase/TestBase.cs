using Demo_Delivery.Domain;
using Demo_Delivery.Infrastructure.Common.Extension;
using Demo_Delivery.Infrastructure.Common.Persistence;
using Demo_Delivery.Infrastructure.Options;
using Demo_Delivery.Persistence.Common.Extension;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using NSubstitute;
using Respawn;
using Serilog;
using Testcontainers.PostgreSql;
using Xunit;
using Xunit.Abstractions;
using ILogger = Serilog.ILogger;

namespace Demo_Delivery.Infrastructure.TestBase;

public class TestFixture<TEntryPoint> : IAsyncLifetime where TEntryPoint : class
{
    private readonly WebApplicationFactory<TEntryPoint> _applicationFactory;
    public IServiceProvider ServiceProvider => _applicationFactory?.Services;

    private PostgreSqlContainer _postgreSqlContainer;
    private Action<IServiceCollection> TestRegistrationServices { get; set; }
    public ILogger Logger { get; set; }

    protected TestFixture()
    {
        _applicationFactory = new WebApplicationFactory<TEntryPoint>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration(AddCustomAppSettings);
            builder.UseEnvironment("Test");
            builder.ConfigureServices(services =>
            {
                TestRegistrationServices?.Invoke(services);
                services.ReplaceSingleton(AddHttpContextAccessor);

                services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                        options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
                        options.DefaultForbidScheme = TestAuthHandler.AuthenticationScheme;
                        options.DefaultScheme = TestAuthHandler.AuthenticationScheme;
                    })
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme,
                        TestAuthHandler.AuthenticationScheme, _ => { });

                services.AddAuthorization(opt =>
                {
                    var adminRolePolicy = new AuthorizationPolicyBuilder()
                        .RequireRole(GlobalConstants.Roles.AdminRoleName)
                        .RequireAuthenticatedUser()
                        .Build();
                    var authenticatedUserPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    opt.AddPolicy(GlobalConstants.Policies.AdminRolePolicy, adminRolePolicy);
                    opt.AddPolicy(GlobalConstants.Policies.AuthenticatedUserPolicy, authenticatedUserPolicy);
                });
            });
        });
    }

    private void AddCustomAppSettings(IConfigurationBuilder configuration)
    {
        //third
        configuration.AddInMemoryCollection(new KeyValuePair<string, string>[]
        {
            new("PostgresSqlOptions:ConnectionString", _postgreSqlContainer.GetConnectionString()),
        });
    }

    public HttpClient CreateHttpClient(bool hasUser, WebApplicationFactoryClientOptions? clientOptions = null)
    {
        return clientOptions != null
            ? _applicationFactory.CreateClient(clientOptions)
            : _applicationFactory.CreateClient();
    }

    private void AddMockAuthentication(IServiceCollection services)
    {
        services.AddAuthorization(opt =>
        {
            var adminRolePolicy = new AuthorizationPolicyBuilder().RequireRole(GlobalConstants.Roles.AdminRoleName)
                .RequireAuthenticatedUser()
                .Build();
            var authenticatedUserPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            opt.AddPolicy(GlobalConstants.Policies.AdminRolePolicy, adminRolePolicy);
            opt.AddPolicy(GlobalConstants.Policies.AuthenticatedUserPolicy, authenticatedUserPolicy);
        });
    }

    public static ILogger CreateLogger(ITestOutputHelper outputHelper)
    {
        if (outputHelper == null) throw new ArgumentNullException(nameof(outputHelper));

        return new LoggerConfiguration().WriteTo.TestOutput(outputHelper).CreateLogger();
    }

    public void RegisterServices(Action<IServiceCollection> services)
    {
        TestRegistrationServices = services;
    }

    protected async Task<T> ExecuteInScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
        var result = await action(scope.ServiceProvider);
        return result;
    }

    private async Task<IHttpContextAccessor> AddHttpContextAccessor(IServiceProvider provider)
    {
        var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
        await using var scope = provider.CreateAsyncScope();
        httpContextAccessorMock.HttpContext = new DefaultHttpContext { RequestServices = scope.ServiceProvider };

        httpContextAccessorMock.HttpContext.Request.Host = new HostString("localhost", Random.Shared.Next(1, 6012));
        httpContextAccessorMock.HttpContext.Request.Scheme = "http";
        return httpContextAccessorMock;
    }

    public async Task InitializeAsync()
    {
        await StartTestContainerAsync();
    }

    private async Task StartTestContainerAsync()
    {
        _postgreSqlContainer = TestContainers.PostgreSqlTestContainer();
        await _postgreSqlContainer.StartAsync();
    }

    private async Task StopTestContainerAsync()
    {
        await _postgreSqlContainer.StopAsync();
    }

    public async Task DisposeAsync()
    {
        await StopTestContainerAsync();
        await _applicationFactory.DisposeAsync();
    }
}

public class TestWriteFixture<TEntryPoint, TWContext> : TestFixture<TEntryPoint>
    where TEntryPoint : class where TWContext : DbContext
{
    public Task<T> ExecuteDbContextAsync<T>(Func<TWContext, Task<T>> action)
    {
        return ExecuteInScopeAsync(sp => action(sp.GetService<TWContext>()));
    }

    public async Task<TEntity> InsertAsync<TEntity>(TEntity entity) where TEntity : class
    {
        await ExecuteDbContextAsync(db =>
        {
            db.Set<TEntity>().Add(entity);

            return db.SaveChangesAsync();
        });

        return entity;
    }
}

public class TestFixture<TEntryPoint, TWContext> : TestWriteFixture<TEntryPoint, TWContext>
    where TEntryPoint : class where TWContext : DbContext
{
}

public class TestFixtureCore<TEntryPoint> : IAsyncLifetime where TEntryPoint : class
{
    private Respawner _respawner;
    private NpgsqlConnection DefaultSqlConnection { get; set; }
    public TestFixture<TEntryPoint> TestFixture { get; }

    public TestFixtureCore(TestFixture<TEntryPoint> integrationTestFixture, ITestOutputHelper outputHelper)
    {
        TestFixture = integrationTestFixture;
    }

    public async Task InitializeAsync()
    {
        await InitSqlAsync();
    }

    public async Task DisposeAsync()
    {
        await ResetSqlAsync();
    }

    private async Task InitSqlAsync()
    {
        //second
        var defaultConnectionString = TestFixture.ServiceProvider.GetService<PostgresSqlOptions>()?.ConnectionString;

        if (defaultConnectionString is not null)
        {
            DefaultSqlConnection = new NpgsqlConnection(defaultConnectionString);
            await DefaultSqlConnection.OpenAsync();

            _respawner = await Respawner.CreateAsync(DefaultSqlConnection,
                new RespawnerOptions { DbAdapter = DbAdapter.Postgres });

            await SeedDataAsync();
        }
    }

    private async Task ResetSqlAsync()
    {
        if (DefaultSqlConnection is not null)
        {
            await _respawner.ResetAsync(DefaultSqlConnection);
        }
    }

    private async Task SeedDataAsync()
    {
        await using var scope = TestFixture.ServiceProvider.CreateAsyncScope();
        var initializer = scope.ServiceProvider.GetService<IInitializerDb>();
        await initializer.InitializeAsync();
    }
}

public abstract class TestBase<TEntryPoint, TWContext> : TestFixtureCore<TEntryPoint>
    where TEntryPoint : class where TWContext : DbContext
{
    protected TestBase(TestFixture<TEntryPoint, TWContext> integrationTestFixture,
        ITestOutputHelper outputHelper = null) : base(integrationTestFixture, outputHelper)
    {
        Fixture = integrationTestFixture;
    }

    public TestFixture<TEntryPoint, TWContext> Fixture { get; }
}