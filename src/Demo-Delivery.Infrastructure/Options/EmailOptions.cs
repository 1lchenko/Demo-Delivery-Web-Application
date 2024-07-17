namespace Demo_Delivery.Application.Options;

public class EmailOptions
{
    public const string Email = "Email";

    public string SmtpHost { get; set; }
    public string EmailFrom { get; set; }
    public string SmtpPassword { get; set; }
    public string SmtpUser { get; set; }
    public int SmtpPort { get; set; }
}