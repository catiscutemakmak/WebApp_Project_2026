namespace hateekub.DTOS
{
public class CreateRoomRequest
{
    public string Game { get; set; }
    public string RoomName { get; set; }
    public string GameMode { get; set; }
    public string GameServer { get; set; }

    public string? MinRank { get; set; }
    public string? MaxRank { get; set; }

    public int MaxPlayer { get; set; }

    public bool RoomPrivacy { get; set; }

    public string? GameRole { get; set; }

    public string? Description { get; set; }

    public DateTime PlayDateTime { get; set; }

    public string? avatar {get;set;}
}
}