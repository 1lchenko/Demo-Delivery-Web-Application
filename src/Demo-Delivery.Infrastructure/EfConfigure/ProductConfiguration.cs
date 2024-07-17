using System.Reflection;
using Demo_Delivery.Domain.Entities.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo_Delivery.Infrastructure.EfConfigure;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.ImageKeys).HasField("_imageKeys");
    }
}