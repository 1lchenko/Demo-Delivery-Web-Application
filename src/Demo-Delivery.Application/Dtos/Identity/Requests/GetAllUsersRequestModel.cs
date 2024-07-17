using Demo_Delivery.Application.Dtos.Account;

namespace Demo_Delivery.Application.Dtos.Identity.Requests;

public class GetAllUsersRequestModel : BaseSortAndFilterRequest
{
    public const int MaxPageSize = 20;
    public List<string>? RoleNames { get; set; }
    
}

 