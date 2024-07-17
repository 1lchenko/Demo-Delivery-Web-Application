using Demo_Delivery.Domain.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo_Delivery.Infrastructure.EfConfigure;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsOne<Address>(x => x.Address);

        builder.Property<int>("_orderStatusId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("OrderStatusId")
            .IsRequired();
 
        builder.HasOne(b => b.OrderStatus)
            .WithMany()
            .HasForeignKey("_orderStatusId")
            .IsRequired();
    }
}