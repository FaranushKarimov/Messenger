using System.ComponentModel.DataAnnotations;

namespace Messenger.Domain.Entities;

public class UserEntity:BaseEntity
{
    [MaxLength(100)]
    public string UserName { get; set; } = null!;

    [MaxLength(255)]
    public string Email { get; set; } = null!;
    public bool IsOnline { get; set; }
    public ICollection<MessageEntity> SentMessages { get; set; } = new List<MessageEntity>();
    public ICollection<GroupUserEntity> GroupUsers { get; set; } = new List<GroupUserEntity>();
}