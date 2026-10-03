using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class VertificationCodeConfig : IEntityTypeConfiguration<VertificationCode>
    {
        public void Configure(EntityTypeBuilder<VertificationCode> builder)
        {
            builder.ToTable("VerificationCodes");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Email)
                .IsRequired()
                .HasMaxLength(320);

            builder.Property(v => v.CodeHash)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(v => v.Purpose)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(v => v.CreatedAt)
                .IsRequired();

            builder.Property(v => v.ExpiresAt)
                .IsRequired();

            builder.Property(v => v.UsedAt)
                .IsRequired(false);

            builder.Property(v => v.Attempts)
                .IsRequired()
                .HasDefaultValue(0);

            builder.HasOne(v => v.User)
                .WithMany()
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.UserId);
        }
    }
}

