namespace Messenger.Domain.Entities;

public class BaseEntity
{
    public string Id { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}