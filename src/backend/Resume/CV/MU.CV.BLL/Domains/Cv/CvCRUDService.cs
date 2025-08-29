using Microsoft.Extensions.DependencyInjection;
using MU.CV.BLL.Common;
using MU.CV.BLL.Common.Models;
using MU.CV.DAL.Common;
using MU.CV.DAL.Entities.Cv;
using MU.CV.DAL.Utils;

namespace MU.CV.BLL.Domains.Cv;

public class CvWriteService(IBaseRepository<CvDAL> repo, IUnitOfWork uow)
    : BaseDtoWrite<CvDAL, CvDto>(repo, uow);

public class CvReadService(IProjectorRepository<CvDAL> repo)
    : BaseDtoRead<CvDAL, CvDto>(repo, dal => dal is {} ? new CvDto(dal.Id, dal.OwnerId, dal.OwnerFullName, dal.Title, dal.About, dal.UniquePath) : null), ICvReadByPath
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

public static class CvExtensions
{
    public static IServiceCollection AddCvServices(this IServiceCollection services)
    {
        services.AddScoped<IDtoWrite<CvDto>, CvWriteService>();
        services.AddScoped<IDtoRead<CvDto>, CvReadService>();
        services.AddScoped<ICvReadByPath, CvReadService>();
        
        return services;
    }
}

public record CvDto(Guid Id, Guid OwnerId, string OwnerFullName, string Title, string About, string UniquePath) : ValueBaseDtoEntity<CvDAL>(Id)
{
    public CvDto() : this(Guid.Empty, Guid.Empty, string.Empty, string.Empty, string.Empty, string.Empty) { }

    public override CvDAL ToDbEntity()
    {
        return new CvDAL()
        {
            Id = Id,
            OwnerId = OwnerId,
            OwnerFullName = OwnerFullName,
            Title = Title,
            About = About,
            UniquePath = UniquePath
        };
    }
}


