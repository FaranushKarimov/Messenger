namespace Messenger.Infrastructure.Settings;

public class EmailSettings
{
    public string From { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string SmtpServer { get; set; } = null!;
    public string Port { get; set; } = null!;
}