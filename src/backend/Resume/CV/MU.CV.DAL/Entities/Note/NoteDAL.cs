using MU.CV.DAL.Common;

namespace MU.CV.DAL.Entities.Note;

public class NoteDAL : BaseHistorianEntity, INote
{
    public string Title { get; set; }
    public string Content { get; set; }
}