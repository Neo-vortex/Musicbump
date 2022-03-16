using System;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MusicPlayer.Controllers;
using MusicPlayer.Models;
using MusicPlayer.Services.UserMusicManager;
using Xunit;



namespace UnitTest;

public class UnitTest1
{
    private readonly Mock<ILogger<HomeController>> _loggerService;
    private readonly Mock<UserManager<IdentityUser>> _usermanagerService;
    private readonly Mock<SignInManager<IdentityUser>> _signinService;
    private readonly Mock<UserMusicService> _userMusicService;
    private readonly Mock<IWebHostEnvironment> _webHostEnvironment;
    private readonly ClaimsPrincipal user;
    public UnitTest1()
    {
        _loggerService = new Mock<ILogger<HomeController>>();
        _userMusicService = new Mock<UserMusicService>();
        _usermanagerService = new Mock<UserManager<IdentityUser>>(new Mock<IUserStore<IdentityUser>>().Object, null,
            null, null, null, null, null, null, null);
        _signinService = new Mock<SignInManager<IdentityUser>>(_usermanagerService.Object,
            Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(), null, null, null,
            null);
        _webHostEnvironment = new Mock<IWebHostEnvironment>();
         user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new(ClaimTypes.NameIdentifier, "SomeValueHere"),
            new(ClaimTypes.Name, "neo.vortex@pm.me")
        },"TestAuthentication"));
    }
    
    [Fact]
    public async void EditorControllerTest()
    {
        var editorController = new EditorController(_loggerService.Object, _signinService.Object,
            _usermanagerService.Object, _userMusicService.Object, _webHostEnvironment.Object);
        editorController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
        Assert.IsType<ViewResult>( await editorController.RenameService("test", "test"));
        
    }
    [Fact]
    public async void PlaylistControllerTest()
    {
        var playlistController = new PlaylistController(_loggerService.Object,_signinService.Object,_usermanagerService.Object,_userMusicService.Object, _webHostEnvironment.Object);
        playlistController.ControllerContext = new ControllerContext();
        playlistController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
        Assert.IsType<ViewResult>( await playlistController.Index("test"));
        Assert.IsType<ViewResult>(  await playlistController.SongFromOnline("test"));
        Assert.IsType<ViewResult>(await playlistController.SongFromLocal("test"));
        Assert.IsType<ViewResult>( await playlistController.OnlineUploadSongService("test", "test", "test"));
        Assert.IsType<ViewResult>( await playlistController.LocalUploadSongService(null,"test","test"));
        Assert.IsType<ViewResult>( await playlistController.play("test", "test"));
    }
    [Fact]
    public async void HomeControllerTest()
    {
        var homeController = new HomeController(_loggerService.Object,_signinService.Object,_usermanagerService.Object,_userMusicService.Object);
        Assert.IsType<ViewResult>( homeController.Index());
        Assert.IsType<ViewResult>( homeController.CreateNewPlaylist());
        Assert.IsType<ViewResult>(await homeController.CreateNewPlaylistService(new UserPlaylist()));
        Assert.IsType<ViewResult>( homeController.Playlist());
        Assert.IsType<ViewResult>( homeController.Error());
    }
}