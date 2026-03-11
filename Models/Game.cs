namespace hateekub.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string GameName { get; set; } = string.Empty;
        public string GameType { get; set; } = string.Empty;
        public int MaxPlayers { get; set; }
        public int MinPlayers { get; set; }

        public bool HasRoles { get; set; }
        public bool HasRanks { get; set; }

        public bool IsDuplicateRole {get; set;}
        public List<GameRole> GameRoles { get; set; } = new();
        public List<GameRank> GameRanks { get; set; } = new();

        public ICollection<UserGame> UserGames { get; set; } = new List<UserGame>();
    }
}