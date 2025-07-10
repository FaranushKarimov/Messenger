using FluentAssertions;
using Messenger.Infrastructure.Services;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Xunit;

namespace Messenger.Tests.Infrastructure;

public class RedisPresenceServiceTests
{
    [Fact]
    public async Task SetUserOnlineAsync_ShouldSetStatusToOnline()
    {
        // Arrange
        var cacheMock = new Mock<IDistributedCache>();
        var service = new RedisPresenceService(cacheMock.Object);

        var userId = "user123";
        var expectedKey = $"user:{userId}:status";
        var expectedValue = System.Text.Encoding.UTF8.GetBytes("online");

        // Act
        await service.SetUserOnlineAsync(userId);

        // Assert
        cacheMock.Verify(c => c.SetAsync(
            expectedKey,
            It.Is<byte[]>(v => v.SequenceEqual(expectedValue)),
            It.IsAny<DistributedCacheEntryOptions>(),
            default
        ), Times.Once);
    }

    [Fact]
    public async Task SetUserOfflineAsync_ShouldSetStatusToOffline()
    {
        var cacheMock = new Mock<IDistributedCache>();
        var service = new RedisPresenceService(cacheMock.Object);

        var userId = "user456";
        var expectedKey = $"user:{userId}:status";
        var expectedValue = System.Text.Encoding.UTF8.GetBytes("offline");

        await service.SetUserOfflineAsync(userId);

        cacheMock.Verify(c => c.SetAsync(
            expectedKey,
            It.Is<byte[]>(v => v.SequenceEqual(expectedValue)),
            It.IsAny<DistributedCacheEntryOptions>(),
            default
        ), Times.Once);
    }

    [Fact]
    public async Task GetUserStatusAsync_ShouldReturnStoredValue()
    {
        // Arrange
        var cacheMock = new Mock<IDistributedCache>();
        var service = new RedisPresenceService(cacheMock.Object);

        var userId = "user789";
        var expectedKey = $"user:{userId}:status";
        var expectedValue = "online";
        var expectedBytes = System.Text.Encoding.UTF8.GetBytes(expectedValue);

        cacheMock.Setup(c => c.GetAsync(expectedKey, default))
            .ReturnsAsync(expectedBytes);

        // Act
        var status = await service.GetUserStatusAsync(userId);

        // Assert
        status.Should().Be(expectedValue);
    }
}