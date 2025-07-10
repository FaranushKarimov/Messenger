using Messenger.Web.Extensions;
using Messenger.Web.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

builder
    .AddSwagger()
    .AddData()
    .AddApplicationServices()
    .AddIntegrationServices()
    .AddOptions()
    .AddBackgroundService()
    .AddBearerAuthentication();

var app = builder.Build();

app.MapHub<ChatHub>("/chatHub");
app.Run();