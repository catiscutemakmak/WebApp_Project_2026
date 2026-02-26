namespace hateekub.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public int UserProfileId { get; set; }
        public bool IsRead { get; set; }
        public UserProfile? UserProfile { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}