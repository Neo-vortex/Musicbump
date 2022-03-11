using System.ComponentModel.DataAnnotations;

namespace MusicPlayer.Models;

public class UserSong
{
    [Key]
    public string Name { get; set; }
    public  long Size { get; set; }
    public  string RelativePath { get; set; }
}