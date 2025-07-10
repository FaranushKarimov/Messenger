using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Infrastructure;

public class MessengerDbContext: DbContext
{
    public MessengerDbContext(DbContextOptions<MessengerDbContext> options):base(options)
    {
        // if (Database.GetPendingMigrations().Any())
        // {
        //     Database.Migrate();
        // }
    }
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<GroupEntity> Groups { get; set; } = null!;
    public DbSet<MessageEntity> Messages { get; set; } = null!;
    public DbSet<GroupUserEntity> GroupUsers { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<GroupUserEntity>()
            .HasKey(gu => new { gu.UserId, gu.GroupId });
    }
    
}