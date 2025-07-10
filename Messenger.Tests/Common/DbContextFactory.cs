using Messenger.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Tests.Common;

public static class DbContextFactory
{
    public static MessengerDbContext CreateInMemory()
    {
        var options = new DbContextOptionsBuilder<MessengerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new MessengerDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}