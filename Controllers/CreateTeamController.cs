using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ProfileWeb.Controllers;

public class CreateTeam : Controller
{
    public IActionResult Create()
    {
        return View();
    }

}
