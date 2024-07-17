using System.Text.Json.Serialization;

namespace Demo_Delivery.Application.Dtos.Account;

public class Account
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public bool EmailConfirmed { get; set; }
    public string JwtToken { get; set; }

    [JsonIgnore] public string RefreshToken { get; set; }
}