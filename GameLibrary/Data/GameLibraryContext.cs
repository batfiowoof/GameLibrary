using GameLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Data
{
    public class GameLibraryContext : DbContext
    {
        public GameLibraryContext(DbContextOptions<GameLibraryContext> options)
            : base(options)
        {
        }

        public DbSet<Game> Game { get; set; }
    }
}
