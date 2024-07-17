using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Domain.Entities.CartAggregate;
using Demo_Delivery.Domain.Entities.CategoryAggregate;
using Demo_Delivery.Domain.Entities.CustomerAggregate;
using Demo_Delivery.Domain.Entities.OrderAggregate;
using Demo_Delivery.Domain.Entities.ProductAggregate;
using Demo_Delivery.Domain.Entities.VoucherAggregate;
using Demo_Delivery.Domain.SeedWork;
using Demo_Delivery.Infrastructure.Common.Extension;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using User = Demo_Delivery.Infrastructure.Identity.Models.User;

namespace Demo_Delivery.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    

    public ApplicationDbContext(IMediator mediator, IUser user, DbContextOptions<ApplicationDbContext> options) :
        base(options)
    {
        _mediator = mediator;
        _user = user;
    }

    private readonly IMediator _mediator;
    private readonly IUser _user;

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<OrderStatus> OrderStatuses { get; set; }

    public DbSet<Cart> Carts { get; set; }

    public DbSet<CartItem> CartItems { get; set; }

    public DbSet<Voucher> Vouchers { get; set; }
    public DbSet<VoucherDiscountType> VoucherDiscountTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                //    entry.Entity.CreatedBy = _user.Id;
                    entry.Entity.CreatedBy = "dwdwd";
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedDate = DateTime.UtcNow;
                     entry.Entity.LastModifiedBy = _user.Id;
                    break;
            }

        await _mediator.DispatchDomainEventsAsync(this);
        ChangeTracker.DetectChanges();
        Console.WriteLine(ChangeTracker.DebugView.ShortView);
        return await base.SaveChangesAsync(cancellationToken);
    }
}