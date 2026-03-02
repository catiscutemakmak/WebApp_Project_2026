namespace hateekub.Models
{
    
    public class UserProfile
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Review> Reviews { get; set; } = new();
        public List<History> Histories { get; set; } = new();
        public List<Notification> Notifications { get; set; } = new();

    }

    
}