using System.Net;
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
    private readonly IWebHostEnvironment _appEnvironment;
    
    
    public PlaylistController(ILogger<HomeController> logger , SignInManager<IdentityUser> SignInManager , UserManager<IdentityUser> UserManager , UserMusicService UserMusicService ,IWebHostEnvironment appEnvironment)
    {
        _appEnvironment = appEnvironment;
        _userMusicService = UserMusicService;
        _userManager = UserManager;
        _signInManager = SignInManager;
        _logger = logger;
    }
    [Authorize]
    [Route("[controller]/onlineuploadpage")]
    public async Task<IActionResult> SongFromOnline(string playlist)
    {
        ViewBag.playlistname = playlist;
        return View();
    }
    
    [Authorize]
    [Route("[controller]/localuploadpage")]
    public async Task<IActionResult> SongFromLocal(string playlist)
    {
        ViewBag.playlistname = playlist;
        return View();
    }
    
        
    [Authorize]
    [HttpPost("[controller]/onlineupload")]
    public async Task<IActionResult> OnlineUploadSongService(string files, string songname, string playlist)
    {
        int result;
        try
        {
            var _random_name = Path.GetRandomFileName();
            using (var client = new WebClient())
            {
                client.DownloadFile(files  , Path.Combine(_appEnvironment.WebRootPath, "songs", _random_name + Path.GetExtension(files)) );
            }
            var _song = new UserSong();
            _song.Name = songname;
            _song.Size =
                new FileInfo(
                    Path.Combine(_appEnvironment.WebRootPath, "songs", _random_name + Path.GetExtension(files))).Length;
            _song.RelativePath =  _random_name + Path.GetExtension(files);
            result =  await  _userMusicService.CreatNewSong(new User() {Email = User.Identity?.Name},
                new UserPlaylist() {Name = playlist}, _song);
        }
        catch (Exception e)
        {
          return  View("Failed");
        }
        return  result >0 ?  View("Successful") : View("Failed");
    }
    [Authorize]
    [HttpPost("[controller]/localupload")]
    public async Task<IActionResult> LocalUploadSongService(List<IFormFile>? files, string songname, string playlist)
    {
        int result;
        try
        {
            var _random_name = Path.GetRandomFileName();
            var output =
                System.IO.File.Create(Path.Combine(_appEnvironment.WebRootPath, "songs", _random_name +  Path.GetExtension(files[0].FileName) ));
            await  files[0].CopyToAsync(output);
            output.Close();
            var _song = new UserSong();
            _song.Name = songname;
            _song.Size = files[0].Length;
            _song.RelativePath = _random_name + Path.GetExtension(files[0].FileName);
               result =  await  _userMusicService.CreatNewSong(new User() {Email = User.Identity?.Name},
                new UserPlaylist() {Name = playlist}, _song);
        }
        catch (Exception e)
        {
            return View("Failed");
        }
        return  result >0 ?  View("Successful") : View("Failed");
    }


    [Route("[controller]/{playlist}/play/{songname}")]
    [Authorize]
    public async Task<IActionResult> play(string playlist, string songname)
    
    {
        try
        {
        
            var _song =( await _userMusicService.GetSongsForPlaylist(new User() {Email = User.Identity?.Name}, playlist))?.Single(song => song.Name == songname)  ;
            if (_song == null)
            {
                return View("Failed");
            }
            ViewBag.filepath = _song.RelativePath;
            ViewBag.songname = _song.Name;
            ViewBag.size = _song.Size;
            ViewBag.playlistname = playlist;
            return View("Index");
        }
        catch (Exception e)
        {
            return View("Failed");
        }

    }
    
    
    [Route("[controller]/{playlist}")]
    [Authorize]
    public async Task<IActionResult> Index(string playlist)
    {
        if (!await _userMusicService.UserHasPlaylist(new User() {Email = User.Identity?.Name}, playlist))
            return View("Failed");
        ViewBag.playlistname = playlist;
        return View();
    }


    
        
    
    [Route("[controller]/{playlist}/renamepage/")]
    [Authorize]
    public  async Task<IActionResult> RenamePage(string playlist )
    {
        ViewBag.playlistname = playlist;
        return View();
    }

    
    
    
    
    

    
    [Route("[controller]/{playlist}/remove/")]
    [Authorize]
    public  async Task<IActionResult> Remove(string playlist )
    {
        if (!await _userMusicService.UserHasPlaylist(new User() {Email = User.Identity?.Name}, playlist))
            return NotFound("You do not have this playlist");
        var result = await _userMusicService.RemovePlaylist(new User() {Email = User.Identity?.Name}, playlist);
        return View(result > 0 ? "Successful" : "Failed");
    }
    
    [Route("[controller]/{playlist}/songs/remove/{songname}")]
    [Authorize]
    public  async Task<IActionResult> RemoveSong(string playlist , string songname)
    {
        var result = await _userMusicService.RemoveSong(new User() {Email = User.Identity?.Name},
            new UserPlaylist() {Name = playlist}, new UserSong() {Name = songname}) ;
        return View(result > 0 ? "Successful" : "Failed");
    }

    
    
    
}