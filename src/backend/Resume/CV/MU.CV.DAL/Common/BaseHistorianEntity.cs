namespace MU.CV.DAL.Common;

public abstract class BaseHistorianEntity : IDbEntity, IHistorianEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid UpdatedBy { get; set; }
}