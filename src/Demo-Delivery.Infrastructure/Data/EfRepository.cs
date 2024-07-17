using Ardalis.Specification.EntityFrameworkCore;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Demo_Delivery.Infrastructure.Data;

public class EfRepository<T> : RepositoryBase<T>, IRepository<T>, IReadRepository<T> where T : Entity
{
   

    public EfRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    
     
}