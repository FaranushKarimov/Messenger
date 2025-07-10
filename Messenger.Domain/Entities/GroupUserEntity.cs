namespace Messenger.Domain.Entities;

public class GroupUserEntity
{
    public string UserId { get; set; } = null!;
    public UserEntity UserEntity { get; set; } = null!;

    public string GroupId { get; set; } = null!;
    public GroupEntity GroupEntity { get; set; } = null!;
}