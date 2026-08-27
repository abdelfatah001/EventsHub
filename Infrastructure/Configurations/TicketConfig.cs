
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class TicketConfig : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.TicketCode)
                   .HasColumnType("varchar(100)")
                   .IsRequired();

            builder.Property(t => t.Status)
                   .IsRequired();

            builder.HasOne(t => t.OrderItem)
                   .WithOne(oi => oi.Ticket)
                   .HasForeignKey<Ticket>(t => t.OrderItemId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.TicketType)
                   .WithMany(tt => tt.Tickets)
                   .HasForeignKey(t => t.TicketTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(t => t.TicketTypeId);
            builder.HasIndex(t => t.OrderItemId);

            builder.HasIndex(t => t.TicketCode)
                   .IsUnique();
        }
    }
}
