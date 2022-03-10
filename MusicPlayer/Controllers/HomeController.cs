using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusicPlayer.Models;

namespace MusicPlayer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(ILogger<HomeController> logger , SignInManager<IdentityUser> SignInManager , UserManager<IdentityUser> UserManager  )
    {
        _userManager = UserManager;
        _signInManager = SignInManager;
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (_signInManager.IsSignedIn(User))
        {
            _logger.LogInformation($"New Request for index from an authorized user : {User.Identity?.Name}");
        }
        else
        {
            _logger.LogInformation($"New Request for index from  an unauthorized user");
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }

    public IActionResult Playlist()
    {
        if (!_signInManager.IsSignedIn(User)) return Unauthorized("you need to login first");
        return View();

    }
}