using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public class RoomController : Controller
{   
    
[Route("game/{gameName}/room/{roomId}")]
public IActionResult Room(string gameName, string roomId)
{
    ViewBag.GameName = gameName;
    ViewBag.RoomId = roomId;
    return View();
}

}