using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlayer.Models;

public class UserPlaylist
{
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public  long _id { get; set; }
    public string Name { get; set; }
    public virtual  List<UserSong> Songs { get; set; }
}