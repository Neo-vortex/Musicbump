using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusicPlayer.Models;
using MusicPlayer.Services.UserMusicManager;

namespace MusicPlayer.Controllers;

public class PlaylistController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly UserMusicService _userMusicService;

    public PlaylistController(ILogger<HomeController> logger , SignInManager<IdentityUser> SignInManager , UserManager<IdentityUser> UserManager , UserMusicService UserMusicService )
    {
        _userMusicService = UserMusicService;
        _userManager = UserManager;
        _signInManager = SignInManager;
        _logger = logger;
    }
    
    // GET
    [Route("[controller]/{playlist}")]
    [Authorize]
    public async Task<IActionResult> Index(string playlist)
    {
        if (!await _userMusicService.UserHasPlaylist(new User() {Email = User.Identity?.Name}, playlist))
            return NotFound("You do not have this playlist");
        ViewBag.Message = "h";
        return Ok("fine");
    }
    [Route("[controller]/{playlist}/rename/{newname}")]
    [Authorize]
    public  async Task<IActionResult> Rename(string playlist, string newname )
    {
        if (!await _userMusicService.UserHasPlaylist(new User() {Email = User.Identity?.Name}, playlist))
            return NotFound("You do not have this playlist");
        var result = await _userMusicService.RenamePlaylist(new User() {Email = User.Identity.Name}, playlist , newname);
        return Ok("fine");
    }
    [Route("[controller]/{playlist}/remove/")]
    [Authorize]
    public  async Task<IActionResult> Remove(string playlist )
    {
        if (!await _userMusicService.UserHasPlaylist(new User() {Email = User.Identity?.Name}, playlist))
            return NotFound("You do not have this playlist");
        var result = await _userMusicService.RemovePlaylist(new User() {Email = User.Identity.Name}, playlist);
        return Ok(result);
    }
}