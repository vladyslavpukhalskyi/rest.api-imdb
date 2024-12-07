using System.Reflection;
using Docker.DotNet.Models;
using Domain.Directors;
using Domain.Movies;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Actor> Actors { get; set; }
    public DbSet<Director> Directors { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Genre> Genres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        ConfigureRelations(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private void ConfigureRelations(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>()
            .HasOne(m => m.Director)
            .WithMany(d => d.Movies)
            .HasForeignKey(m => m.DirectorId);

        modelBuilder.Entity<Movie>()
            .HasOne(m => m.Genre)
            .WithMany(g => g.Movies)
            .HasForeignKey(m => m.GenreId);

        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Actors)
            .WithMany(a => a.Movies)
            .UsingEntity(j => j.ToTable("MovieActors"));
    }
}