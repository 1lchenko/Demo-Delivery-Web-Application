using System.Reflection;
using Demo_Delivery.Domain;
using Demo_Delivery.Domain.Entities.CartAggregate;
using Demo_Delivery.Domain.Entities.CategoryAggregate;
using Demo_Delivery.Domain.Entities.CustomerAggregate;
using Demo_Delivery.Domain.Entities.ProductAggregate;
using Demo_Delivery.Domain.Entities.VoucherAggregate;
using Demo_Delivery.Domain.SeedWork;
using Demo_Delivery.Infrastructure.Common.Persistence;
using Demo_Delivery.Infrastructure.Identity.Models;
using Demo_Delivery.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Demo_Delivery.Infrastructure.Data;

public class InitializerDb : IInitializerDb
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly IEnumerable<IInicialData> _inicialDatas;

    public InitializerDb(ApplicationDbContext context, IEnumerable<IInicialData> inicialDatas,
        RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    {
        _context = context;
        _inicialDatas = inicialDatas;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task InitializeAsync()
    {
        foreach (var initialData in _inicialDatas)
        {
            if (DataSetIsEmpty(initialData.EntityType))
            {
                await _context.AddRangeAsync(initialData.GetData());
            }
        }

        await SeedRoles();
        await SeedUsers();
        await SeedAdmin();

        await _context.SaveChangesAsync();
    }

    private async Task SeedVouchers()
    {
        if (!await _context.Vouchers.AnyAsync())
        {
            var vouchers = new List<Voucher>
            {
                new("Voucher1", 100, 100, VoucherDiscountType.Value, DateTime.UtcNow.AddDays(100), DateTime.UtcNow),
                new("Voucher2", 300, 100, VoucherDiscountType.Value, DateTime.UtcNow.AddDays(100), DateTime.UtcNow),
                new("Voucher3", 4000, 100, VoucherDiscountType.Value, DateTime.UtcNow.AddDays(100),
                    DateTime.UtcNow),
                new("Voucher4", 100000, 100, VoucherDiscountType.Value, DateTime.UtcNow.AddDays(100),
                    DateTime.UtcNow),
                new("Voucher5", 100, 100, VoucherDiscountType.Percentage, DateTime.UtcNow.AddDays(100),
                    DateTime.UtcNow),
                new("Voucher6", 300, 100, VoucherDiscountType.Percentage, DateTime.UtcNow.AddDays(100),
                    DateTime.UtcNow),
                new("Voucher7", 4000, 100, VoucherDiscountType.Percentage, DateTime.UtcNow.AddDays(100),
                    DateTime.UtcNow),
                new("Voucher8", 100000, 100, VoucherDiscountType.Percentage, DateTime.UtcNow.AddDays(100),
                    DateTime.UtcNow)
            };

            await _context.AddRangeAsync(vouchers);
        }
    }

    private async Task SeedRoles()
    {
        if (!await _roleManager.Roles.AnyAsync())
        {
            var roles = new List<IdentityRole>
            {
                new() { Name = GlobalConstants.Roles.UserRoleName },
                new() { Name = GlobalConstants.Roles.AdminRoleName }
            };

            foreach (var role in roles) await _roleManager.CreateAsync(role);
        }
    }

    private async Task SeedAdmin()
    {
        if (!await _context.Users.AnyAsync(x => x.Email == "cortezz1ty@gmail.com"))
        {
            var user = new User
            {
                Email = "cortezz1ty@gmail.com",
                UserName = "Admin2",
                PhoneNumber = "+380507174432",
                EmailConfirmed = true
            };
            await _userManager.CreateAsync(user, "string");

            await _userManager.AddToRoleAsync(user, GlobalConstants.Roles.AdminRoleName);

            var customer = new Customer("Admin", user.Id, user.Email);
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            var cart = new Cart(customer.Id);
            await _context.Carts.AddAsync(cart);
        }
    }

    private async Task SeedUsers()
    {
        if (!await _context.Users.AnyAsync())
        {
            var user = new User
            {
                Id = TestUserSettings.UserId, Email = TestUserSettings.UserEmail, UserName = TestUserSettings.Name
            };
            await _userManager.CreateAsync(user);
            var customer = new Customer(user.UserName, user.Id, user.Email);
            await _context.AddAsync(customer);
            await _context.SaveChangesAsync();

            var cart = new Cart(customer.Id);
            cart.AddCartItem(new Product("Name", "Description Product", 100, 100, 100, 100, true, new Category("Name"),
                null));
            await _context.AddAsync(cart);
            await _context.SaveChangesAsync();
        }
    }

    private bool DataSetIsEmpty(Type type)
    {
        var setMethod = GetType()
            .GetMethod(nameof(GetSet), BindingFlags.Instance | BindingFlags.NonPublic)
            .MakeGenericMethod(type);

        var set = setMethod.Invoke(this, null);

        var countMethod = typeof(Queryable).GetMethods()
            .First(x => x.Name == nameof(Queryable.Count) && x.GetParameters().Length == 1)
            .MakeGenericMethod(type);

        var result = (int)countMethod.Invoke(null, new[] { set });
        return result == 0;
    }

    private DbSet<TEntity> GetSet<TEntity>() where TEntity : class
    {
        return _context.Set<TEntity>();
    }
}