using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class OrganizerConfig : IEntityTypeConfiguration<Organizer>
    {
        public void Configure(EntityTypeBuilder<Organizer> builder)
        { 
            builder.Property(o => o.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        // One User can have at most one Organizer profile.
        builder.HasOne(o => o.User)
            .WithOne()
            .HasForeignKey<Organizer>(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // One Organizer can manage many Events.
        builder.HasMany(o => o.Events)
            .WithOne()
            .HasForeignKey("OrganizerId")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(o => o.UserId)
            .IsUnique();
        }
    }   
}
