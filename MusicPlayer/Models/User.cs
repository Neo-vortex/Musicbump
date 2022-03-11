using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlayer.Models;

public class User
{
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public  long _id { get; set; }
    public string? Email { get; set; }

    public virtual List<UserPlaylist> UserPlaylist { get; set; }
}