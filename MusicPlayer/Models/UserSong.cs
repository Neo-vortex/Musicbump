using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlayer.Models;

public class UserSong
{
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public  long _id { get; set; }
    public string Name { get; set; }
    public  long Size { get; set; }
    public  string RelativePath { get; set; }
}