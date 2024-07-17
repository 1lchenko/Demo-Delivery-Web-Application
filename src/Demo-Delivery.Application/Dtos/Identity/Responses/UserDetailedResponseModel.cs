namespace Demo_Delivery.Application.Dtos.Identity;

public class UserDetailedResponseModel
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
    public DateTime CreatedOn { get; set; }  
    public DateTime? UpdatedOn { get; set; }   
    public bool EmailConfirmed { get; set; }
    public DateTime? LastPasswordReset { get; set; }  
    public bool IsActive { get; set; }
    public DateTime? LastLoginTime { get; set; } 
}