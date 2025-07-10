using FluentAssertions;
using Messenger.Domain.Entities;
using Messenger.Infrastructure.Repositories;
using Messenger.Tests.Common;
using Xunit;

namespace Messenger.Tests.Infrastructure;

public class MessageRepositoryTests
{
    [Fact]
    public async Task AddAsync_ShouldAddMessageToDatabase()
    {
        var context = DbContextFactory.CreateInMemory();
        var repository = new MessageRepository(context);

        var message = new MessageEntity
        {
            Id = Guid.NewGuid(),
            FromUserId = "user1",
            ToUserId = "user2",
            Content = "Hello, world!",
            SentAt = DateTime.UtcNow,
            IsRead = false
        };

        // Act
        await repository.AddAsync(message);

        // Assert
        var savedMessage = await context.Messages.FindAsync(message.Id);
        savedMessage.Should().NotBeNull();
        savedMessage!.Content.Should().Be("Hello, world!");
    }
    
    [Fact]
    public async Task EditAsync_ShouldUpdateMessageContent()
    {
        var context = DbContextFactory.CreateInMemory();
        var repository = new MessageRepository(context);

        var message = new MessageEntity
        {
            Id = Guid.NewGuid(),
            FromUserId = "user1",
            ToUserId = "user2",
            Content = "Original message",
            SentAt = DateTime.UtcNow,
            IsRead = false
        };

        await repository.AddAsync(message);

        await repository.EditAsync(message, "Updated content");

        var updatedMessage = await context.Messages.FindAsync(message.Id);
        updatedMessage.Should().NotBeNull();
        updatedMessage!.Content.Should().Be("Updated content");
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldRemoveMessageFromDatabase()
    {
        var context = DbContextFactory.CreateInMemory();
        var repository = new MessageRepository(context);

        var message = new MessageEntity
        {
            Id = Guid.NewGuid(),
            FromUserId = "user1",
            ToUserId = "user2",
            Content = "Message to delete",
            SentAt = DateTime.UtcNow,
            IsRead = false
        };

        await repository.AddAsync(message);
        
        await repository.DeleteAsync(message);

    
        var deletedMessage = await context.Messages.FindAsync(message.Id);
        deletedMessage.Should().BeNull();
    }
    
    [Fact]
    public async Task GetHistoryAsync_ShouldReturnMessagesBetweenUsers()
    {
        var context = DbContextFactory.CreateInMemory();
        var repository = new MessageRepository(context);

        var message1 = new MessageEntity
        {
            Id = Guid.NewGuid(),
            FromUserId = "user1",
            ToUserId = "user2",
            Content = "Hi there",
            SentAt = DateTime.UtcNow,
            IsRead = false
        };

        var message2 = new MessageEntity
        {
            Id = Guid.NewGuid(),
            FromUserId = "user2",
            ToUserId = "user1",
            Content = "Hello!",
            SentAt = DateTime.UtcNow,
            IsRead = false
        };

        var unrelatedMessage = new MessageEntity
        {
            Id = Guid.NewGuid(),
            FromUserId = "user3",
            ToUserId = "user4",
            Content = "Unrelated message",
            SentAt = DateTime.UtcNow,
            IsRead = false
        };

        await repository.AddAsync(message1);
        await repository.AddAsync(message2);
        await repository.AddAsync(unrelatedMessage);

        var history = await repository.GetHistoryAsync("user1", "user2");

        history.Should().HaveCount(2);
        history.Should().Contain(m => m.Content == "Hi there");
        history.Should().Contain(m => m.Content == "Hello!");
    }
}