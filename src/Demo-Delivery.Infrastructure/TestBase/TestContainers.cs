using Demo_Delivery.Persistence.Common.Extension;
using Microsoft.AspNetCore.Authentication;
using Testcontainers.PostgreSql;

namespace Demo_Delivery.Infrastructure.TestBase;

public static class TestContainers
{
    public static PostgreSqlContainerOptions PostgreSqlContainerOptions { get; }

    static TestContainers()
    {
        var conf = ConfigurationHelper.GetConfiguration();
        PostgreSqlContainerOptions = conf.GetOptions<PostgreSqlContainerOptions>(nameof(PostgreSqlContainerOptions));
    }
    
    public static PostgreSqlContainer PostgreSqlTestContainer()
    {
        var baseBuilder = new PostgreSqlBuilder()
            .WithLabel("Key", "Value")
            .WithPassword(PostgreSqlContainerOptions.Password); 

        var builder = baseBuilder
            .WithImage(PostgreSqlContainerOptions.ImageName)
            .WithName(PostgreSqlContainerOptions.Name)
            .WithPortBinding(PostgreSqlContainerOptions.Port, true)
            .Build();

        return builder;
    }
}

public sealed class PostgreSqlContainerOptions
{
    public string Name { get; set; } = "postgreSql_Dima"; //"postgreSql_" + Guid.NewGuid().ToString("D");
    public int Port { get; set; } = 5432;
    public string ImageName { get; set; } = "postgres:latest";
    public string Password { get; set; } = Guid.NewGuid().ToString("D");
}
