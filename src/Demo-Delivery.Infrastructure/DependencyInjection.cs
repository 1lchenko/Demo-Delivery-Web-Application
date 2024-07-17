using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using Amazon.S3;
using Demo_Delivery.Application.Common.Abstractions.Services;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.Options;
using Demo_Delivery.Domain;
using Demo_Delivery.Infrastructure.Common.Extension;
using Demo_Delivery.Infrastructure.Common.Logging;
using Demo_Delivery.Infrastructure.Common.Persistence;
using Demo_Delivery.Infrastructure.Data;
using Demo_Delivery.Infrastructure.Identity;
using Demo_Delivery.Infrastructure.Identity.Models;
using Demo_Delivery.Infrastructure.Options;
using Demo_Delivery.Infrastructure.Services;
using Demo_Delivery.Persistence.Common;
using Demo_Delivery.Persistence.Common.Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Demo_Delivery.Infrastructure;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder,
        IConfiguration configuration)
    {
        builder.Services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.Email))
            .AddIdentityServices(configuration)
            .AddDatabase(configuration)
            .AddRepositories()
            .AddServices();
             

        builder.AddSerilog(builder.Environment);

        return builder;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PostgresSqlOptions>(configuration.GetSection(PostgresSqlOptions.PostgresOptions));
        services.AddSingleton(x => x.GetRequiredService<IOptions<PostgresSqlOptions>>().Value);

        return services.AddDbContext<ApplicationDbContext>(opt =>
            {
                var connectionString = services.BuildServiceProvider()
                    .GetRequiredService<PostgresSqlOptions>()
                    .ConnectionString;
                //fifth
                opt.UseNpgsql(connectionString,
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            })
            .AddTransient<IInitializerDb, InitializerDb>();
    }

     

     
    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
            .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services.AddSingleton<IFileService, FileService>()
            .AddSingleton<IEmailService, EmailService>()
            .AddScoped<IJwtService, JwtService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IRoleService, RoleService>()
            .AddAWSService<IAmazonS3>()
            .AddScoped<IUser, AspNetUser>();
    }

    private static IServiceCollection AddIdentityServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtAppSettingOptions = configuration.GetSection(JWTOptions.JWT);
        var secretKey = jwtAppSettingOptions[nameof(JWTOptions.Secret)];

        if (string.IsNullOrEmpty(secretKey))
        {
            return services;
        }

        var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        var r = jwtAppSettingOptions[nameof(JWTOptions.Issuer)];
        services.Configure<JWTOptions>(options =>
        {
            options.Issuer = jwtAppSettingOptions[nameof(JWTOptions.Issuer)];
            options.Audience = jwtAppSettingOptions[nameof(JWTOptions.Audience)];
            options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        });

        services.Configure<RefreshTokenOptions>(configuration.GetSection(RefreshTokenOptions.RefreshToken));

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtAppSettingOptions[nameof(JWTOptions.Issuer)],
            ValidateAudience = true,
            ValidAudience = jwtAppSettingOptions[nameof(JWTOptions.Audience)],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            RequireExpirationTime = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configureOptions =>
            {
                configureOptions.RequireHttpsMetadata = true;
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JWTOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

        services.AddAuthorization(options =>
        {
            var adminRolePolicy = new AuthorizationPolicyBuilder().RequireRole(GlobalConstants.Roles.AdminRoleName)
                .RequireAuthenticatedUser()
                
                .RequireActiveUser(services.BuildServiceProvider())
                .Build();
            var authenticatedUserPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                .RequireActiveUser(services.BuildServiceProvider())
                .Build();
            options.AddPolicy(GlobalConstants.Policies.AdminRolePolicy, adminRolePolicy);
            options.AddPolicy(GlobalConstants.Policies.AuthenticatedUserPolicy, authenticatedUserPolicy);
        });

        return services;
    }
}