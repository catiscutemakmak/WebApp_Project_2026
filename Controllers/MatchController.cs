using Microsoft.AspNetCore.Mvc;
public class MatchController : Controller
{
    [HttpPost]
    public IActionResult Index(string gameKey, string gameID)
    {
        ViewBag.GameKey = gameKey;
        ViewBag.GameID = gameID;
        return View();
    }
}