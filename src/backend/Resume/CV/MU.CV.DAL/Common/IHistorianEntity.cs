namespace MU.CV.DAL.Common;

public interface IHistorianEntity
{   
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
    Guid CreatedBy { get; set; }
    Guid UpdatedBy { get; set; }
}