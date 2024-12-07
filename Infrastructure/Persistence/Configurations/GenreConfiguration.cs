using Domain.Movies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new GenreId(x));

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnType("varchar(255)");
        
        builder.HasMany(x => x.Movies)
            .WithOne(x => x.Genre)
            .HasForeignKey(x => x.GenreId)
            .HasConstraintName("fk_movies_genres_id")
            .OnDelete(DeleteBehavior.Restrict);
    }
}