namespace hateekub.Models
{
public class RoomPlayer
{
    public int Id { get; set; }

    public int RoomId { get; set; }
    public Room? Room { get; set; }

    public int UserId { get; set; }
    public UserProfile? User { get; set; }

    public DateTime JoinedAt { get; set; }

    public GameRole? Role { get; set; }

    public int? RoleId { get; set; }

    public GameRank? Rank { get; set; }

    public string? Avatar  { get; set; }
    public int? RankId { get; set; }
    public bool IsReady { get; set; }
    public bool IsInQueue { get; set; }

    public PlayerStatus Status { get; set; }
}
}