namespace hateekub.Models
{
    public class ProfileGame
    {
        public int Id { get; set; }

        public string GameName { get; set; } = string.Empty;
        public string Rank { get; set; } = string.Empty;

        // Foreign Key
        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; } = null!;
    }
}