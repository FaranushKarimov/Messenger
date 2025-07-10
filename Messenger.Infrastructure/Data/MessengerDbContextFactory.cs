using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Messenger.Infrastructure.Data;

public class MessengerDbContextFactory : IDesignTimeDbContextFactory<MessengerDbContext>
{
    public MessengerDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MessengerDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=MessengerDb;Trusted_Connection=True;TrustServerCertificate=True;");

        return new MessengerDbContext(optionsBuilder.Options);
    }
}