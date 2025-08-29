using MU.CV.DAL.Common;
using MU.CV.DAL.Entities.Cv;

namespace MU.CV.DAL.Entities.JobExperience;

public class JobExperienceDAL : BaseHistorianEntity, IJobExperience
{
    public Guid CvId { get; set; }
    public CvDAL Cv { get; set; }

    public string PositionDescription { get; set; }
    public DateOnly LengthOfWorkFrom { get; set; }
    public DateOnly? LengthOfWorkTo { get; set; }
    public TimeSpan PositionExpCount =>
        (LengthOfWorkTo ?? DateOnly.FromDateTime(DateTime.UtcNow))
        .ToDateTime(TimeOnly.MinValue) - LengthOfWorkFrom.ToDateTime(TimeOnly.MinValue);

    public string PositionTitle { get; set; }
    public string JobCompanyName { get; set; }
    public TimeSpan JobExpCount => PositionExpCount;
    public string JobCompanyLogo { get; set; }

    public bool IsIT { get; set; } = true;
    public bool IsFreelance { get; set; } = false;
}


