namespace Messenger.Domain.Entities;

public class GroupEntity:BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<GroupUserEntity> GroupUsers { get; set; } = new List<GroupUserEntity>();
    public ICollection<MessageEntity> Messages { get; set; } = new List<MessageEntity>();
}