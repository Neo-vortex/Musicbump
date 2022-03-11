using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MusicPlayer.Controllers;

public class PlaylistController : Controller
{
    // GET
    [Route("[controller]/{playlist}")]
    [Authorize]
    public IActionResult Index(string playlist)
    {
        return View();
    }
}