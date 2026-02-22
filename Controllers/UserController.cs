using Microsoft.AspNetCore.Mvc;

namespace hateekub.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Review()
        {
            return View();
        }

        public IActionResult History()
        {
            return View();
        }
    }

    
}