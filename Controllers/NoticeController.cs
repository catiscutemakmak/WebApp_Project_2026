using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace testttt.Controllers;

public class NoticeController : Controller
{
    public IActionResult Notice()
    {
        return View();
    }
}