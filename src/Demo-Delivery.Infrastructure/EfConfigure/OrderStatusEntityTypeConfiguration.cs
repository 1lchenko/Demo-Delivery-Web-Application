using Demo_Delivery.Domain.Entities.OrderAggregate;
using Demo_Delivery.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo_Delivery.Infrastructure.EfConfigure;

public class OrderStatusEntityTypeConfiguration : IEntityTypeConfiguration<OrderStatus>
{
    public void Configure(EntityTypeBuilder<OrderStatus> builder)
    {
        builder.ToTable("OrderStatuses");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(b => b.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}