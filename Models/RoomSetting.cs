namespace hateekub.Models
{
public class RoomSetting
    {
        public int Id { get; set; }

        public Room? Room { get; set; }

        public int RoomId { get; set; }

        public string? MinRank { get; set; } = string.Empty;

        public string? MaxRank { get; set; } = string.Empty;

        public bool AllowDuplicateRole { get; set; }

        public bool IsPrivate { get; set; }

        public int MaxPlayer { get; set; }
    }
}