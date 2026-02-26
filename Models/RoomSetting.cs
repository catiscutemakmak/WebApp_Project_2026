
namespace hateekub.Models
{
public class RoomSetting
    {
        public int Id { get; set; }

        public Room? Room { get; set; }

        public int RoomId { get; set; }

        public int MinRank { get; set; }

        public int MaxRank { get; set; }

        public Boolean AllowDuplicateRole { get; set; }

        public Boolean IsPrivate { get; set; }

        public int MaxPlayer { get; set; }
    }
}