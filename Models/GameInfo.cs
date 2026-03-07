namespace hateekub.Models
{public class UserGame
{
    public int Id { get; set; }
    public int UserProfileId { get; set; }
    public UserProfile User { get; set; } = null!;
    public int GameId { get; set; }
    public Game Game { get; set; } = null!;
    public string? InGameName { get; set; }

    public string? RankTier { get; set; }
    public int? RankDivision { get; set; }
    public DateTime? RankLastUpdated { get; set; }
    public string? ExtraDataJson { get; set; } // ข้อมูลเฉพาะเกม
}
}