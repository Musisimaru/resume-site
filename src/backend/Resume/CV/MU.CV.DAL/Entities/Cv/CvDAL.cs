using MU.CV.DAL.Common;
using MU.CV.DAL.Entities.JobExperience;

namespace MU.CV.DAL.Entities.Cv;

public class CvDAL : BaseHistorianEntity, ICv
{
    public Guid OwnerId { get; set; }
    public string OwnerFullName { get; set; }
    public string Title { get; set; }
    public string About { get; set; }
    public string UniquePath { get; set; }

    public List<JobExperienceDAL> JobExperiences { get; set; } = new();
}


