using Messenger.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Messenger.Infrastructure.HostedService;

public class MessageMonitorService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MessageMonitorService> _logger;

    public MessageMonitorService(IServiceProvider serviceProvider, ILogger<MessageMonitorService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MessengerDbContext>();
            var redis = scope.ServiceProvider.GetRequiredService<RedisPresenceService>();
            var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();

            var threshold = DateTime.UtcNow.AddMinutes(-5);
            
            var unreadMessages = await dbContext.Messages
                .Where(m => !m.IsRead && m.SentAt < threshold)
                .Include(m => m.ToUser) 
                .ToListAsync(stoppingToken);

            foreach (var message in unreadMessages)
            {
                var userStatus = await redis.GetUserStatusAsync(message.ToUserId);
                if (userStatus != "online")
                {
                    var recipientEmail = message.ToUser?.Email;
                    if (!string.IsNullOrEmpty(recipientEmail))
                    {
                        await emailService.SendEmailAsync(
                            recipientEmail,
                            "Новое непрочитанное сообщение",
                            $"У вас новое сообщение от пользователя {message.FromUserId}: \"{message.Content}\"");
                    }
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}