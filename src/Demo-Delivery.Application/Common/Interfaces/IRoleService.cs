using Demo_Delivery.Application.Dtos.Role;

namespace Demo_Delivery.Application.Common.Interfaces;

public interface IRoleService
{
    public Task<List<string>> GetAllAsync();
}