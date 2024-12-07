using Domain.Movies;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new MovieId(x));

        builder.Property(x => x.Title).IsRequired().HasColumnType("varchar(255)");
        builder.Property(x => x.ReleaseYear).IsRequired();
        
        builder.HasOne(x => x.Genre)
            .WithMany()
            .HasForeignKey(x => x.GenreId)
            .HasConstraintName("fk_movies_genres_id")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.Director)
            .WithMany()
            .HasForeignKey(x => x.DirectorId)
            .HasConstraintName("fk_movies_directors_id")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(x => x.Actors)
            .WithMany(x => x.Movies)
            .UsingEntity(j => j.ToTable("MovieActors"));
    }
}