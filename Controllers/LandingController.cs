using Microsoft.AspNetCore.Mvc;

namespace hateekub.Controllers
{
    public class LandingController : Controller
    {
        public IActionResult Landing()
        {
            return View();
        }

    }

    
}