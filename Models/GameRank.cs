namespace hateekub.Models
{
    public class GameRank
    {
        public int Id { get; set; }
        public string RankName { get; set; } = string.Empty;
        public int GameId { get; set; }
        public Game? Game { get; set; } = null!; 
        public string RankImageUrl { get; set; } = string.Empty;

    }    
}