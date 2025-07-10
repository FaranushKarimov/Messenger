namespace Messenger.Application.DTOs;

public record ChatHistoryDto(
    string UserA,
    string UserB,
    List<MessageDto> Messages
);