using Messenger.Applications.Abstractions;
using Messenger.Infrastructure;
using Messenger.Infrastructure.HostedService;
using Messenger.Infrastructure.Repositories;
using Messenger.Infrastructure.Services;
using Messenger.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Web.Extensions;

public static class ServiceCollectionsExtensions
{
    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        return builder;
    }

    public static WebApplicationBuilder AddData(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<MessengerDbContext>(options =>
            options.UseSqlServer(connectionString,
                b => b.MigrationsAssembly("Messenger.Infrastructure"))); 

        return builder;
    }

    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IMessageRepository, MessageRepository>();
        builder.Services.AddScoped<EmailService>();
        builder.Services.AddScoped<RedisPresenceService>();
        return builder;
    }
    
    public static WebApplicationBuilder AddIntegrationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("Redis");
        });
        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
        return builder;
    }

    public static WebApplicationBuilder AddBackgroundService(this WebApplicationBuilder builder)
    {
        builder.Services.AddHostedService<MessageMonitorService>();
        return builder;
    }

    public static WebApplicationBuilder AddBearerAuthentication(this WebApplicationBuilder builder)
    {
        return builder;
    }

    public static WebApplicationBuilder AddOptions(this WebApplicationBuilder builder)
    {
        return builder;
    }
}