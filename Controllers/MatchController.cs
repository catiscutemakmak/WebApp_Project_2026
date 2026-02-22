
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using testttt.Models;

namespace testttt.Controllers;

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

        public IActionResult Test()
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
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}