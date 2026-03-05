namespace hateekub.DTOS
{
public class RoomSettingDTO
{
    public int MinRank { get; set; } 
    public int MaxRank { get; set; }
    public bool AllowDuplicateRole { get; set; }
    public bool IsPrivate { get; set; }
    public int MaxPlayer { get; set; }

    }
}