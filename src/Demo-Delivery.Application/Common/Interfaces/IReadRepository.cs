using Ardalis.Specification;
using Demo_Delivery.Domain.SeedWork;

namespace Demo_Delivery.Application.Common.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T>  where T : Entity
{
    
}