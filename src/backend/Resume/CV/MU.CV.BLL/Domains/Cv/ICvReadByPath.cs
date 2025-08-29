namespace MU.CV.BLL.Domains.Cv;

public interface ICvReadByPath
{
    Task<CvDto?> GetByUniquePathAsync(string uniquePath, CancellationToken ct = default);
}


