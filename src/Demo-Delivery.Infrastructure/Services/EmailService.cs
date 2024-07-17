using Demo_Delivery.Application.Common.Abstractions.Services;
using Demo_Delivery.Application.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Demo_Delivery.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;

    public EmailService(IOptions<EmailOptions> appSettings)
    {
        _emailOptions = appSettings.Value;
    }

    public async Task SendAsync(string to, string subject, string html, string? from = null)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(from ?? _emailOptions.EmailFrom));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html)
        {
            Text = html
        };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_emailOptions.SmtpHost, _emailOptions.SmtpPort, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_emailOptions.SmtpUser, _emailOptions.SmtpPassword);
        await smtp.SendAsync(email);
    }
}