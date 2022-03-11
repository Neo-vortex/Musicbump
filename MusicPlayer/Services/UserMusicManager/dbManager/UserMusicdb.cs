using Microsoft.EntityFrameworkCore;
using MusicPlayer.Models;

namespace MusicPlayer.Services.UserMusicManager.dbManager;

public sealed class UserMusicdb : DbContext
{
    public DbSet<User> _users { get; set; } 
    public UserMusicdb()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        SQLitePCL.Batteries.Init();
        optionsBuilder.UseSqlite("Data Source=./playlists.db;");
        base.OnConfiguring(optionsBuilder);
    }
}