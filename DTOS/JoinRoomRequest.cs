namespace hateekub.DTOS
{
public class JoinRoomRequest
{
    public string? RoleName { get; set; } = string.Empty;
    public string? Avatar { get; set; }

    public int? RankId {get; set;}
}
}