using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // ลอง login ด้วย Email
        var result = await _signInManager.PasswordSignInAsync(
            model.Username, 
            model.Password, 
            isPersistent: false, 
            lockoutOnFailure: true
        );

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }

        if (result.IsLockedOut)
        {
            ViewBag.Error = "Account locked out. Try again in 5 minutes.";
        }
        else
        {
            ViewBag.Error = "Invalid email or password.";
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new IdentityUser
        {
            UserName = model.Username,
            Email = model.Username
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            // Auto login หลัง register
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        // แสดง errors จาก Identity
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Landing", "Landing");
    }
}


