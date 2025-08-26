using MU.CV.BLL.Common;
using MU.CV.DAL.Common;
using MU.CV.DAL.Entities.Note;
using MU.CV.DAL.Utils;

namespace MU.CV.BLL.Domains.Notes;

public class NotesCRUDService(IBaseRepository<NoteDAL> repository, IUnitOfWork unitOfWork)
    : BaseCRUDService<NoteDAL>(repository, unitOfWork);