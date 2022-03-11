using System.ComponentModel.DataAnnotations;

namespace MusicPlayer.Models;

public class UserPlaylist
{
    [Key]
    public string Name { get; set; }
    public virtual  List<UserSong> Songs { get; set; }
}