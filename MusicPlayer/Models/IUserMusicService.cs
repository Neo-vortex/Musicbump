namespace MusicPlayer.Models;

public interface IUserMusicService
{
    public Task<List<UserPlaylist>?> GetPlaylistForUser(User user);
}