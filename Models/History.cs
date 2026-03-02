namespace hateekub.Models
{
    public class History
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int UserProfileId { get; set; }
        public UserProfile? User { get; set; } = null!;
        public Room? Room { get; set; } = null!;
    }
}