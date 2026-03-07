namespace hateekub.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int UserProfileId { get; set; }
        public int ReviewerId { get; set; }
        public int Rating { get; set; }
        public UserProfile? User { get; set; } = null!;
        public UserProfile? Reviewer { get; set; } = null!;

    }
}