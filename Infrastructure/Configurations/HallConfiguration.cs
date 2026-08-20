using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class HallConfiguration : IEntityTypeConfiguration<Hall>
    {
        public void Configure(EntityTypeBuilder<Hall> builder)
        {

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired();

            builder.Property(x => x.Capacity)
                       .IsRequired();

            builder.Property(x => x.Address)
                   .IsRequired()
                   .HasMaxLength(200);

        }

    }
}
