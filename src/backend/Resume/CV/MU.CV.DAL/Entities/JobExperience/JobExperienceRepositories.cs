using MU.CV.DAL.Common;
using MU.CV.DAL.DataContext;

namespace MU.CV.DAL.Entities.JobExperience;

public class JobExperienceHistorianRepository(CVDbContext context) : BaseCVHistorianRepository<JobExperienceDAL>(context);
public class JobExperienceProjectorRepository(CVDbContext context) : BaseCVProjectorRepository<JobExperienceDAL>(context);


