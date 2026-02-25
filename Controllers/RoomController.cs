using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;


    
[Route("game/{gameName}/room")]
public class RoomController : Controller
{
    [Route("{roomId:int}")]
    public IActionResult Room(string gameName, int roomId)
    {
        ViewBag.GameName = gameName;
        ViewBag.RoomId = roomId;

        return View();
    }
}