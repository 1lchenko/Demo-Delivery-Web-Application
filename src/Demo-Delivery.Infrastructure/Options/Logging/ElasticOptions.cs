namespace Demo_Delivery.Application.Options.Logging;

public class ElasticOptions
{
    public const string Elastic = "ElasticOptions";
    public bool Enabled { get; set; }
    public string ElasticServiceUrl { get; set; }
    public string ElasticSearchIndex { get; set; }
}