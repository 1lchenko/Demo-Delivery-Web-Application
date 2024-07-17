using Ardalis.Specification;
using Demo_Delivery.Domain.SeedWork;

namespace Demo_Delivery.Application.Common.Interfaces;

public interface IRepository<T> : IRepositoryBase<T>  where T : Entity
{
}