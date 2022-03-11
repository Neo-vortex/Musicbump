using System.ComponentModel.DataAnnotations;

namespace MusicPlayer.Models;

public class User
{
    [Key]
    public string? Email { get; set; }

    public virtual ICollection<UserPlaylist> UserPlaylist { get; set; }
}