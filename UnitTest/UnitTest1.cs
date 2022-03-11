using System.Security.Claims;
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
    public UnitTest1()
    {
        _loggerService = new Mock<ILogger<HomeController>>();
        _userMusicService = new Mock<UserMusicService>();
        _usermanagerService = new Mock<UserManager<IdentityUser>>(new Mock<IUserStore<IdentityUser>>().Object, null,
            null, null, null, null, null, null, null);
        _signinService = new Mock<SignInManager<IdentityUser>>(_usermanagerService.Object,
            Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(), null, null, null,
            null);
    }
    [Fact]
    public async void Test1()
    {
        var controller = new HomeController(_loggerService.Object,_signinService.Object,_usermanagerService.Object,_userMusicService.Object);
        Assert.IsType<ViewResult>( controller.Index());
        Assert.IsType<ViewResult>( controller.CreateNewPlaylist());
        Assert.IsType<ViewResult>(await controller.CreateNewPlaylistService(new UserPlaylist()));
        Assert.IsType<ViewResult>( controller.Playlist());
        Assert.IsType<ViewResult>( controller.Error());
        
     
    }
}