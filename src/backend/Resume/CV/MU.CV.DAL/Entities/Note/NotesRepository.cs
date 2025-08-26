using MU.CV.DAL.Common;
using MU.CV.DAL.DataContext;
using MU.CV.DAL.Entities.Note;

namespace MU.CV.DAL.Repositories;

public class NotesRepository(CVDbContext context) : BaseCVRepository<NoteDAL>(context);