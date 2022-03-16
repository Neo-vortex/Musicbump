using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusicPlayer.Models;
using MusicPlayer.Services.UserMusicManager;

namespace MusicPlayer.Controllers;

public class EditorController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly UserMusicService _userMusicService;
    private readonly IWebHostEnvironment _appEnvironment;

    public EditorController(ILogger<HomeController> logger, SignInManager<IdentityUser> SignInManager,
        UserManager<IdentityUser> UserManager, UserMusicService UserMusicService, IWebHostEnvironment appEnvironment)
    {
        _appEnvironment = appEnvironment;
        _userMusicService = UserMusicService;
        _userManager = UserManager;
        _signInManager = SignInManager;
        _logger = logger;
    }
    // GET

    [HttpPost]    
    [Authorize]
    public  async Task<IActionResult> RenameService(string playlist, string newname )
    {
        if (!await _userMusicService.UserHasPlaylist(new User() {Email = User.Identity?.Name}, playlist))
            return View("Failed");
        var result = await _userMusicService.RenamePlaylist(new User() {Email = User.Identity?.Name}, playlist , newname);
        return View(result > 0 ? "Successful" : "Failed");
    }

}