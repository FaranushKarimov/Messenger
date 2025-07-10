using Messenger.Application.DTOs;
using Messenger.Application.Mappers;
using Messenger.Applications.Abstractions;
using Messenger.Domain.Entities;
using Messenger.Infrastructure.Services;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.Web.Hubs;

public class ChatHub : Hub
{
    private readonly RedisPresenceService _presenceService;
    private readonly IMessageRepository _messageRepository;

    public ChatHub(RedisPresenceService presenceService, IMessageRepository messageRepository)
    {
        _presenceService = presenceService;
        _messageRepository = messageRepository;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            await _presenceService.SetUserOnlineAsync(userId);
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            await _presenceService.SetUserOfflineAsync(userId);
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(SendMessageRequestDto request)
    {
        var message = new MessageEntity
        {
            Id = Guid.NewGuid(),
            FromUserId = request.FromUserId,
            ToUserId = request.ToUserId,
            Content = request.Content,
            SentAt = DateTime.UtcNow,
            IsRead = false
        };

        await _messageRepository.AddAsync(message);

        var messageDto = message.ToDto();

        await Clients.User(request.ToUserId).SendAsync("ReceiveMessage", messageDto);
    }

    public async Task EditMessage(Guid messageId, string newContent)
    {
        var message = await _messageRepository.GetByIdAsync(messageId);
        if (message == null)
        {
            // Можно отправить сообщение об ошибке клиенту
            return;
        }

        await _messageRepository.EditAsync(message, newContent);
        await Clients.User(message.ToUserId).SendAsync("MessageEdited", messageId, newContent);
    }

    public async Task DeleteMessage(Guid messageId)
    {
        var message = await _messageRepository.GetByIdAsync(messageId);
        if (message == null)
        {
            return;
        }

        await _messageRepository.DeleteAsync(message);
        await Clients.User(message.ToUserId).SendAsync("MessageDeleted", messageId);
    }

    public async Task GetHistory(string fromUserId, string toUserId)
    {
        var history = await _messageRepository.GetHistoryAsync(fromUserId, toUserId);
        var historyDtos = history.Select(m => m.ToDto()).ToList();

        await Clients.Caller.SendAsync("ReceiveHistory", historyDtos);
    }
}
