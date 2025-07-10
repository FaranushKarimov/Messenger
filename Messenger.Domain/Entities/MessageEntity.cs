namespace Messenger.Domain.Entities;

public class MessageEntity
{
    public Guid Id { get; set; }

    public string FromUserId { get; set; } = null!;
 //   public UserEntity FromUser { get; set; } = null!; 

    public string ToUserId { get; set; } = null!;
    public UserEntity ToUser { get; set; } = null!;

    public string? GroupId { get; set; }
    public GroupEntity? GroupEntity { get; set; }

    public string Content { get; set; } = null!;
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
}