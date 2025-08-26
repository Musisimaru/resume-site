using MU.CV.DAL.Common;

namespace MU.CV.DAL.Entities.Note;

public class NoteDAL : BaseDbEntity, INote
{
    public string Title { get; set; }
    public string Content { get; set; }
}