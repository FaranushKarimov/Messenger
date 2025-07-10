namespace Messenger.Application.DTOs;

public record MessageDto(
    Guid Id,
    string FromUserId,
    string ToUserId,
    string Content,
    DateTime SentAt,
    bool IsRead
);