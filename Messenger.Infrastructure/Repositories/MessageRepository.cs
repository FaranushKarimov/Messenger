using Messenger.Applications.Abstractions;
using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Infrastructure.Repositories;

public class MessageRepository:IMessageRepository
{
    private readonly MessengerDbContext _dbContext;

    public MessageRepository(MessengerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MessageEntity?> GetByIdAsync(Guid messageId)
    {
        return await _dbContext.Messages.FindAsync(messageId);
    }

    public async Task AddAsync(MessageEntity message)
    {
        await _dbContext.Messages.AddAsync(message);
        await _dbContext.SaveChangesAsync();
    }

    public async Task EditAsync(MessageEntity message, string newContent)
    {
        message.Content = newContent;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(MessageEntity message)
    {
        _dbContext.Messages.Remove(message);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<MessageEntity>> GetHistoryAsync(string fromUserId, string toUserId)
    {
        return await _dbContext.Messages
            .Where(m => (m.FromUserId == fromUserId && m.ToUserId == toUserId) ||
                        (m.FromUserId == toUserId && m.ToUserId == fromUserId))
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }
}