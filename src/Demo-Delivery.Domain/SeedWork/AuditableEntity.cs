namespace Demo_Delivery.Domain.SeedWork;

public class AuditableEntity : Entity
{
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public string? LastModifiedBy { get; set; }
}