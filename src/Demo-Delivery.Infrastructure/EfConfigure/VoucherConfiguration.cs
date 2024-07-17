using Demo_Delivery.Domain.Entities.VoucherAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo_Delivery.Infrastructure.EfConfigure;

public class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
{
    public void Configure(EntityTypeBuilder<Voucher> builder)
    {
        builder.Ignore(b => b.DomainEvents);
        

        builder.Property<int>("_voucherDiscountTypeId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("DiscountTypeId")
            .IsRequired();

        builder.HasOne(b => b.VoucherDiscountType)
            .WithMany()
            .HasForeignKey("_discountTypeId");
    }
}