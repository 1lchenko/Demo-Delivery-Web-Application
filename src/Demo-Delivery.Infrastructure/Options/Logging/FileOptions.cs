namespace Demo_Delivery.Application.Options.Logging;

public class FileOptions
{
    public const string File = "FileOptions";
    public bool Enabled { get; set; }
    public string Path { get; set; }
    public string Interval { get; set; }
}