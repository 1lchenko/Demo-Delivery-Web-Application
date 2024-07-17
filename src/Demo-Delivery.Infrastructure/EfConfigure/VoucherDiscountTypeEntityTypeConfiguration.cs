using Demo_Delivery.Domain.Entities.VoucherAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo_Delivery.Infrastructure.EfConfigure;

public class VoucherDiscountTypeEntityTypeConfiguration : IEntityTypeConfiguration<VoucherDiscountType>
{
    public void Configure(EntityTypeBuilder<VoucherDiscountType> builder)
    {
        builder.ToTable("VoucherDiscountTypes");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(b => b.Name)
            .HasMaxLength(100)
            .IsRequired();
    }
}