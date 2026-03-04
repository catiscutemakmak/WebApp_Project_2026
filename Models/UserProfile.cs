using Microsoft.AspNetCore.Identity;
namespace hateekub.Models
{
    
    public class UserProfile
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;

        public IdentityUser User { get; set; } = null!; 
        public string Nickname { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ProfileImagePath { get; set; }
        public List<Review> Reviews { get; set; } = new();
        
        public List<History> Histories { get; set; } = new();
        public List<Notification> Notifications { get; set; } = new();

        public ICollection<UserGame> UserGames { get; set; } = new List<UserGame>();
        public List<ProfileGame> ProfileGames { get; set; } = new List<ProfileGame>();
    }

    
}