using Microsoft.Extensions.DependencyInjection;
using MU.CV.BLL.Common;
using MU.CV.BLL.Common.Models;
using MU.CV.BLL.Common.User;
using MU.CV.DAL.Common;
using MU.CV.DAL.Entities.Cv;
using MU.CV.DAL.Utils;

namespace MU.CV.BLL.Domains.Cv;

public class CvAuthorizedWriteService(IHistorianRepository<CvDAL> repo, IUnitOfWork uow, ICurrentUser user)
    : BaseAuthorizedWrite<CvDAL, CvDto>(repo, uow, user,
        (stored, edited) =>
        {
            stored.OwnerId = edited.OwnerId;
            stored.OwnerFullName = edited.OwnerFullName;
            stored.Title = edited.Title;
            stored.About = edited.About;
        });

public class CvAuthorizedReadService(IProjectorRepository<CvDAL> repo, ICurrentUser user)
    : BaseAuthorizedRead<CvDAL, CvDto>(repo, user, (dal => new CvDto(dal.Id, dal.OwnerId, dal.OwnerFullName, dal.Title, dal.About)));

public static class CvExtentions
{
    public static IServiceCollection AddCvServices(this IServiceCollection services)
    {
        services.AddScoped<IDtoWrite<CvDto>, CvAuthorizedWriteService>();
        services.AddScoped<IDtoRead<CvDto>, CvAuthorizedReadService>();
        
        return services;
    }
}

public record CvDto(Guid Id, Guid OwnerId, string OwnerFullName, string Title, string About) : ValueBaseDtoEntity<CvDAL>(Id)
{
    public CvDto() : this(Guid.Empty, Guid.Empty, string.Empty, string.Empty, string.Empty) { }

    public override CvDAL ToDbEntity()
    {
        return new CvDAL()
        {
            Id = Id,
            OwnerId = OwnerId,
            OwnerFullName = OwnerFullName,
            Title = Title,
            About = About
        };
    }
}


