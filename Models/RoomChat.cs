using Microsoft.AspNetCore.Identity;

namespace hateekub.Models
{
public class RoomChat
{
    public int Id { get; set; }

    public int RoomId { get; set; }
    public Room? Room { get; set; }

    public string UserId { get; set; } = string.Empty;
    public IdentityUser? User { get; set; }

    public string Message { get; set; } = string.Empty;

    public DateTime SentAt { get; set; }
}
}