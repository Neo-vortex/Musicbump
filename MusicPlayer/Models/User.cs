using System.ComponentModel.DataAnnotations;

namespace MusicPlayer.Models;

public class User
{
    [Key]
    public string? Email { get; set; }
    public  List<UserPlaylist>  UserPlaylist { get; set; }
}