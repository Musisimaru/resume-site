using MU.CV.DAL.Common;
using MU.CV.DAL.DataContext;

namespace MU.CV.DAL.Entities.Cv;

public class CvHistorianRepository(CVDbContext context) : BaseCVHistorianRepository<CvDAL>(context);
public class CvProjectorRepository(CVDbContext context) : BaseCVProjectorRepository<CvDAL>(context);


