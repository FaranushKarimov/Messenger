using Messenger.Domain.Entities;

namespace Messenger.Applications.Abstractions;

public interface IMessageRepository
{
    Task<MessageEntity?> GetByIdAsync(Guid messageId);
    Task AddAsync(MessageEntity message);
    Task EditAsync(MessageEntity message, string newContent);
    Task DeleteAsync(MessageEntity message);
    Task<List<MessageEntity>> GetHistoryAsync(string fromUserId, string toUserId);
}