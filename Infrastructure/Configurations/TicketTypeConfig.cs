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


            builder.HasOne(tt => tt.Occurence)
                   .WithMany(o => o.TicketTypes)
                   .HasForeignKey(tt => tt.OccurenceId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(tt => tt.OccurenceId);
        }

    }
}
