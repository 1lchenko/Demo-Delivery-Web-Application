using Demo_Delivery.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo_Delivery.Infrastructure.EfConfigure;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.OwnsMany<RefreshToken>(u => u.RefreshTokens,
            refreshTokens => { refreshTokens.ToTable("RefreshTokens"); });
    }
}