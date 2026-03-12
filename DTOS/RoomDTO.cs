using hateekub.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace hateekub.DTOS
{
    public class RoomDTO
{
    public int RoomId { get; set; }
    public string GameName { get; set; } = string.Empty;
    public string RoomName { get; set; } = string.Empty;
    public string OwnerUsername { get; set; } = string.Empty;
    public int OwnerId { get; set; } 
    public string GameMode { get; set; } = string.Empty;

    public bool IsOwner  { get; set; }
    public RoomSettingDTO? RoomSetting { get; set; }

    public List<PlayerDTO> Players { get; set; } = new();

    public string MyStatus { get; set; }

    public RoomStatus RoomStatus {get; set;}

    public DateTime PlayTime {get; set;}

    public string? Description {get; set;}
}
}