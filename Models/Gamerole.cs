namespace hateekub.Models
{
public class GameRole
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public int GameId { get; set; }
        public Game? Game { get; set; } = null!; 
    }
}