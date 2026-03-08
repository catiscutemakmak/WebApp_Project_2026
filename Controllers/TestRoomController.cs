using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace testttt.Controllers;

public class TestRoomController : Controller
{
    public IActionResult TestRoom()
    {
        return View();
    }
}