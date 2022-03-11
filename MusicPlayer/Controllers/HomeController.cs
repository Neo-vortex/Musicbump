using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusicPlayer.Models;
using MusicPlayer.Services.UserMusicManager;

namespace MusicPlayer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly UserMusicService _userMusicService;

    public HomeController(ILogger<HomeController> logger , SignInManager<IdentityUser> SignInManager , UserManager<IdentityUser> UserManager , UserMusicService UserMusicService  )
    {
        _userMusicService = UserMusicService;
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

    [Authorize]
    public IActionResult Playlist()
    {
        return View();

    }

    [Authorize]
    public async Task <IActionResult> CreateNewPlaylistService(UserPlaylist model)
    {
        try
        {
          await _userMusicService.CreateNewPlaylist(new User() {Email = User.Identity?.Name}, model.Name == null ? throw new ArgumentException("") : model);
            return View("Successful");
        }
        catch (Exception e)
        {
            _logger.LogWarning($"creating new playlist failed with : {e.Message}");
            return View("Failed");
        }

    
    }

    [Authorize]
    public IActionResult CreateNewPlaylist()
    {
        return View();
    }
}