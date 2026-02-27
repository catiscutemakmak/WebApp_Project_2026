namespace hateekub.Models
{
public class RoomQueue
{
    public int Id { get; set; }

    public int RoomId { get; set; }
    public Room? Room { get; set; }

    public int UserId { get; set; }
    public UserProfile? User { get; set; }

    public DateTime QueuedAt { get; set; }
}
}