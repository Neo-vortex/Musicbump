using MusicPlayer.Models;
using MusicPlayer.Services.UserMusicManager.dbManager;
using NuGet.Protocol;

namespace MusicPlayer.Services.UserMusicManager;

public class UserMusicService : IUserMusicService
{
    private UserMusicdb _db;
    private object _lock = new();
    public UserMusicService()
    {
        _db = new UserMusicdb();
    }

    
    
    public Task<int> CreateNewSongUser(User user)
    {
        return Task.Run(() =>
        {
            lock (_lock)
            {
                if (_db._users.Any(usr => usr.Email == user.Email)) return 0;
                _db._users.Add(new User() {Email = user.Email, UserPlaylist = new List<UserPlaylist>()});
                return _db.SaveChanges();
            }
        });
    }
    
    
    public Task<int> CreateNewPlaylist(User user, UserPlaylist userPlaylist)
    {
        return Task.Run(() =>
        {
            lock (_lock)
            {
                if (!_db._users.Any(usr => usr.Email == user.Email))
                {
                    throw new ArgumentException("user doesn't exists");
                }

                var _tmp = _db._users.Single(usr => usr.Email == user.Email);
                if (_tmp.UserPlaylist == null)
                {
                    _db._users.Single(usr => usr.Email == user.Email).UserPlaylist = new List<UserPlaylist>()
                    {
                        new UserPlaylist()
                            {Name = userPlaylist.Name, Songs = new List<UserSong>()}
                    };
                    return _db.SaveChanges();
                }
                _db._users.Single(usr => usr.Email == user.Email).UserPlaylist.Add(new UserPlaylist()
                    {Name = userPlaylist.Name, Songs = new List<UserSong>()});
                return _db.SaveChanges();
            }

        });
    }
    
    
    public Task<int> AddNewSong(User user, UserPlaylist playlist, UserSong song)
    {
        return Task.Run(() =>
        {
            lock (_lock)
            {
              if ( _db._users.Single(usr => usr.Email == user.Email) == null )
              {
                  throw new ArgumentException("user doesn't exists");
              }
              if (_db._users.Single(usr => usr.Email == user.Email).UserPlaylist.All(playlst => playlst.Name != playlist.Name))
              {
                  throw new ArgumentException("playlist doesn't exists");
              }
              _db._users.Single(usr => usr.Email == user.Email).UserPlaylist.Single(list => list.Name == playlist.Name).Songs.Add(song);
              return _db.SaveChanges();
            }
        });
    }
    
    public Task<List<UserPlaylist>?> GetPlaylistForUser(User user)
    {
       return Task.Run(() =>
       {
           lock (_lock)
           {
               var x = _db._users.Single(usr => usr.Email == user.Email);
                return !_db._users.Any() ? null : _db._users.Single(usr => usr.Email == user.Email)?.UserPlaylist;
            }
        });
    }
}