namespace Demo_Delivery.Application.Options.Logging;

public class LogOptions
{
    public const string Log = "LogOptions";
    public string Level { get; set; }
    public ElasticOptions Elastic { get; set; }
    public FileOptions File { get; set; }
    public string LogTemplate { get; set; }
}