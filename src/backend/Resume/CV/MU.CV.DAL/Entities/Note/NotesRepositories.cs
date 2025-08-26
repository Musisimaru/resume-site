using MU.CV.DAL.Common;
using MU.CV.DAL.DataContext;
using MU.CV.DAL.Entities.Note;

namespace MU.CV.DAL.Repositories;

public class NotesHistorianRepository(CVDbContext context) : BaseCVHistorianRepository<NoteDAL>(context);
public class NotesProjectorRepository(CVDbContext context) : BaseCVProjectorRepository<NoteDAL>(context);