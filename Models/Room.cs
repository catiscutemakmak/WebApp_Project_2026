namespace hateekub.Models
{
public class Room
{
    public string RoomName { get; set; } = string.Empty;

    public int Id {get; set;}
    public Game? Game { get; set; }

    public int GameId {get; set;}

    public string Server {get; set;} = string.Empty;

    public UserProfile? RoomOwner {get; set;}

    public int OwnerId {get; set;}

    public string Description {get; set;} = string.Empty;

    public DateTime PlayDateTime { get; set; }

    public string GameMode { get; set; } = string.Empty;

    public RoomSetting? RoomSetting { get; set; }


    public ICollection<RoomPlayer> Players { get; set; } = new List<RoomPlayer>(); 

    public IEnumerable<RoomPlayer> ActivePlayers =>
        Players.Where(p => !p.IsInQueue);

    public IEnumerable<RoomPlayer> Queue =>
        Players.Where(p => p.IsInQueue);
    public ICollection<RoomChat> Chats { get; set; } = new List<RoomChat>();

    }



}