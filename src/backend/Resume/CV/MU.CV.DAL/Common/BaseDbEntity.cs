namespace MU.CV.DAL.Common;

public abstract class BaseDbEntity : IDbEntity
{
    public Guid Id { get; set; }
}