using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

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

}
