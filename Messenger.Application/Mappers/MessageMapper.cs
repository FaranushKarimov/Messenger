using Messenger.Application.DTOs;
using Messenger.Domain.Entities;

namespace Messenger.Application.Mappers;

public static class MessageMapper
{
    public static MessageDto ToDto(this MessageEntity message)
    {
        return new MessageDto(
            message.Id,
            message.FromUserId,
            message.ToUserId,   
            message.Content,
            message.SentAt,
            message.IsRead
        );
    }
}