namespace hateekub.DTOS
{
public class RoomSettingDTO
{
    public string MaxRank { get; set; } = string.Empty;
    public string MinRank { get; set; } = string.Empty;
    public bool AllowDuplicateRole { get; set; }
    public bool IsPrivate { get; set; }
    public int MaxPlayer { get; set; }

    }
}