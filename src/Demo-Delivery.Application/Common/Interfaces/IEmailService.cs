namespace Demo_Delivery.Application.Common.Abstractions.Services;

public interface IEmailService
{
    public Task SendAsync(string to, string subject, string html, string? from = null);
}