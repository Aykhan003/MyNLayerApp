namespace MyNLayerApp.Entity.Entities.Common;

public class AuditEntity : BaseEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
