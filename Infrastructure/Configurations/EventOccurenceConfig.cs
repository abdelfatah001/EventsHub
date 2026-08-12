using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class EventOccurenceConfig : IEntityTypeConfiguration<EventOccurence>
    {
        public void Configure(EntityTypeBuilder<EventOccurence> builder)
        {
            builder.HasKey(eo => eo.Id);

            builder.Property(eo => eo.StartDate)
                   .IsRequired();

            builder.Property(eo => eo.EndDate)
                   .IsRequired();
            builder.Property(eo => eo.Location)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasOne(eo => eo.Event)
                   .WithMany(e => e.Events)
                   .HasForeignKey(eo => eo.EventId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
