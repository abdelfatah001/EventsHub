using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class TicketTypeConfig : IEntityTypeConfiguration<TicketType>
    {

        public void Configure (EntityTypeBuilder<TicketType> builder) 
        {
            builder.HasKey(tt => tt.Id);

            builder.Property(tt => tt.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(tt => tt.Description)
                .HasMaxLength(500);

            builder.Property(tt => tt.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");


            builder.HasMany(tt => tt.Tickets)
                   .WithOne(t => t.TicketType)
                   .HasForeignKey(t => t.TicketTypeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
