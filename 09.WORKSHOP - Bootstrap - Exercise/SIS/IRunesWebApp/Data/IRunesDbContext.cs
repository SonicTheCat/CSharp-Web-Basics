namespace IRunesWebApp.Data
{
    using IRunesWebApp.Models;
    using Microsoft.EntityFrameworkCore;

    public class IRunesDbContext : DbContext
    {
        public IRunesDbContext() { }

        public IRunesDbContext(DbContextOptions options)
        : base(options)
        {
        }

        public DbSet<Album> Albums { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Track> Tracks { get; set; }

        public DbSet<AlbumTrack> AlbumTracks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(@"Server=.;Database=IRunesWebApp;Trusted_Connection=True")
                    .UseLazyLoadingProxies(); 
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlbumTrack>().HasKey(x => new { x.TrackId, x.AlbumId });
        }
    }
}