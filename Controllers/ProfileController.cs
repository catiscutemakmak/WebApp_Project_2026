using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProfileWeb.Models;

namespace ProfileWeb.Controllers;

public class ProfileController : Controller
{
    public IActionResult EditProfile()
    {
        return View();
    }

    public IActionResult ViewProfile()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
