using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public class MatchController : Controller
{   
    
[Route("game/{gameName}")]
public IActionResult Match(string gameName)
{
    ViewBag.GameName = gameName;
    return View();
}

        [HttpPost]
    public IActionResult Index(string gameKey, string gameID)
    {
        ViewBag.GameKey = gameKey;
        ViewBag.GameID = gameID;
        return View();
    }
}