using Domain.Actors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ActorConfiguration : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .HasConversion(x => x.Value, x => new ActorId(x))
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
                .WithMany(x => x.Actors)
                .UsingEntity(j => j.ToTable("MovieActors"));
        }
    }
}