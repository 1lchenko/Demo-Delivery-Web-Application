namespace Demo_Delivery.Infrastructure.Options;

public class PostgresSqlOptions
{
    public const string PostgresOptions = "PostgresSqlOptions";
    public string ConnectionString { get; set; }
}