using Microsoft.EntityFrameworkCore;
using MusicPlayer.Models;
using MusicPlayer.Services.UserMusicManager.dbManager;
using NuGet.Protocol;

namespace MusicPlayer.Services.UserMusicManager;

public class UserMusicService
{
    private UserMusicdb _db;
    private object _lock = new();
    public UserMusicService()
    {
        _db = new UserMusicdb();
    }

    public Task<int> RenamePlaylist(User user, string playlistname , string newplaylistname)
    {
        return  Task.Run(() =>
        {
            lock (_lock)
            {
                var _user = _db._users.Include(usr => usr.UserPlaylist).Single(usr => usr.Email == user.Email);
                _user.UserPlaylist[_user.UserPlaylist.FindIndex(list => list.Name == playlistname)].Name = newplaylistname ;
                _db._users.Update(_user);
                return _db.SaveChanges();
            }

        });
    }

    public Task<int> RemovePlaylist(User user , string playlistname)
    {
      return  Task.Run(() =>
        {
            lock (_lock)
            {
                var _user = _db._users.Include(usr => usr.UserPlaylist).Single(usr => usr.Email == user.Email);
                if ( _user.UserPlaylist.Any(list => list.Name == playlistname))    _user.UserPlaylist.RemoveAll(list => list.Name == playlistname);
                _db._users.Update(_user);
                return _db.SaveChanges();
            }

        });
    }

    public Task<bool> PlaylistHasSong(User user , string playlistname , string songnname)
    {
    return    Task.Run(() =>
        {
            lock (_lock)
            {
                if (_db._users.Any(usr => usr.Email == user.Email)) return false;
                return _db._users.Include(usr => usr.UserPlaylist).Single(usr => usr.Email == user.Email).UserPlaylist.Any(list => list.Name == playlistname) && _db._users.Include(usr => usr.UserPlaylist).Single(usr => usr.Email == user.Email).UserPlaylist.Single(list => list.Name == playlistname).Songs.Any(song => song.Name == songnname);
            }
        });
    }


    public Task<bool> UserHasPlaylist(User user, string playlistname)
    {
      return  Task.Run(() =>
        {
            lock (_lock)
            {
                if (! _db._users.Any(usr => usr.Email == user.Email)) return false;
                return _db._users.Include(usr => usr.UserPlaylist).Single(usr => usr.Email == user.Email).UserPlaylist
                    .Any(list => list.Name == playlistname);
            }
        });
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

                var x = _db._users.Single(usr => usr.Email == user.Email);
                if (x.UserPlaylist.Any(list => list.Name == userPlaylist.Name)) return 0;
                x.UserPlaylist.Add(new UserPlaylist()
                    {Name = userPlaylist.Name, Songs = new List<UserSong>()});
                return _db.SaveChanges();
            }

        });
    }



    public Task<int> RemoveSong(User user, UserPlaylist playlist, UserSong song)
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
                var xx =   _db._users.Include(usr => usr.UserPlaylist).ThenInclude(lst => lst.Songs).Single(usr => usr.Email == user.Email).UserPlaylist.Single(list => list.Name == playlist.Name);
                var xxx = xx.Songs;
                xxx.RemoveAll(sng => sng.Name == song.Name);
                return _db.SaveChanges();
            }
        });
    }
    
    public Task<int> CreatNewSong(User user, UserPlaylist playlist, UserSong song)
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
           var xx =   _db._users.Include(usr => usr.UserPlaylist).ThenInclude(lst => lst.Songs).Single(usr => usr.Email == user.Email).UserPlaylist.Single(list => list.Name == playlist.Name);
           var xxx = xx.Songs;
           xxx.Add(song);
           return _db.SaveChanges();
            }
        });
    }


    public Task<List<UserSong>> GetSongsForPlaylist(User user , string playlist)
    {
     return   Task.Run(() =>
        {
            lock (_lock)
            {
                if ( ! _db._users.Any(usr => usr.Email == user.Email) )
                {
                    throw new ArgumentException("user doesn't exists");
                }
                if (_db._users.Single(usr => usr.Email == user.Email).UserPlaylist.All(playlst => playlst.Name != playlist))
                {
                    throw new ArgumentException("playlist doesn't exists");
                }
                return _db._users.Include(usr => usr.UserPlaylist).ThenInclude(lst => lst.Songs)  .Single(usr => usr.Email == user.Email)
                    .UserPlaylist.Single(list => list.Name == playlist).Songs;
            }
        });
    }

    public Task<List<UserPlaylist>?> GetPlaylistForUser(User user)
    {
       return Task.Run(() =>
       {
           lock (_lock)
           {
               return !_db._users.Any() ? null : _db._users.Include(m => m.UserPlaylist).Single(usr => usr.Email == user.Email).UserPlaylist;
           }
       });
    }
}