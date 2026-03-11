namespace hateekub.DTOS
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? RoomId { get; set; }
        public string? RoomName { get; set; }
        public string? GameName { get; set; }
        public string? ActorUserName { get; set; }
        public string? ActorProfileImage { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
