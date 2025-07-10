using System.Net;
using System.Net.Mail;
using Messenger.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Messenger.Infrastructure.Services;

public class EmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpClient = new SmtpClient(_settings.SmtpServer)
        {
            Port = int.Parse(_settings.Port),
            Credentials = new NetworkCredential(_settings.From, _settings.Password),
            EnableSsl = true
        };

        var message = new MailMessage(_settings.From, toEmail, subject, body);

        await smtpClient.SendMailAsync(message);
    }
}