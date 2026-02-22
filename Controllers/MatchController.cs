using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public class MatchController : Controller
{
    public IActionResult Match()
    {
        return View();
    }
        public IActionResult Room()
    {
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