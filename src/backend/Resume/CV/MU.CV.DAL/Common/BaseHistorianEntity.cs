namespace MU.CV.DAL.Common;

public abstract class BaseHistorianEntity : BaseDbEntity, IHistorianEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid UpdatedBy { get; set; }
}