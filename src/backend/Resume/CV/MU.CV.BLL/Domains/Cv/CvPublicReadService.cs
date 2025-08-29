using MU.CV.DAL.Common;
using MU.CV.DAL.Entities.Cv;

namespace MU.CV.BLL.Domains.Cv;

public class CvPublicReadService(IProjectorRepository<CvDAL> repo) : ICvReadByPath
{
    public async Task<CvDto?> GetByUniquePathAsync(string uniquePath, CancellationToken ct = default)
    {
        var spec = new PublicSpec() { Criteria = it => it.UniquePath == uniquePath };
        var items = await repo.GetAllAsync(dal => new CvDto(dal.Id, dal.OwnerId, dal.OwnerFullName, dal.Title, dal.About, dal.UniquePath), ct, spec);
        return items.FirstOrDefault();
    }

    private class PublicSpec : ISpecification<CvDAL>
    {
        public System.Linq.Expressions.Expression<Func<CvDAL, bool>>? Criteria { get; init; }
        public List<System.Linq.Expressions.Expression<Func<CvDAL, object>>> Includes { get; } = [];
        public System.Linq.Expressions.Expression<Func<CvDAL, object>>? OrderBy => it => it.Id;
        public System.Linq.Expressions.Expression<Func<CvDAL, object>>? OrderByDesc => null;
        public int? Skip { get; }
        public int? Take { get; }
    }
}


