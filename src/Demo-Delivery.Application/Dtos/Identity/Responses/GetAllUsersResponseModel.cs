namespace Demo_Delivery.Application.Dtos.Identity.Responses;

public class GetAllUsersResponseModel
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string RoleNames { get; set; }
    public bool EmailConfirmed { get; set; }
    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
    
     
}