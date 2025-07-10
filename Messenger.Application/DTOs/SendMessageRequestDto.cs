namespace Messenger.Application.DTOs;

public record SendMessageRequestDto(
    string FromUserId,
    string ToUserId,
    string Content
);