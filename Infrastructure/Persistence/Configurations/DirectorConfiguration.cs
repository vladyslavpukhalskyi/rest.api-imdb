using Domain.Directors;
using Domain.Movies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class DirectorConfiguration : IEntityTypeConfiguration<Director>
    {
        public void Configure(EntityTypeBuilder<Director> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .HasConversion(x => x.Value, x => new DirectorId(x))
                .IsRequired();
            
            builder.Ignore(x => x.Name);
            
            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(x => x.BirthDate)
                .IsRequired()
                .HasColumnType("date");
            
            builder.HasMany(x => x.Movies)
                .WithOne(x => x.Director)
                .HasForeignKey(x => x.DirectorId)
                .HasConstraintName("fk_movies_directors_id")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}