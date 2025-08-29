namespace MU.CV.DAL.Entities.Cv;

public interface ICv
{
    Guid OwnerId { get; set; }
    string OwnerFullName { get; set; }
    string Title { get; set; }
    string About { get; set; }
    string UniquePath { get; set; }
}