using Microsoft.Extensions.DependencyInjection;
using MU.CV.BLL.Common;
using MU.CV.BLL.Common.Models;
using MU.CV.DAL.Common;
using MU.CV.DAL.Entities.JobExperience;
using MU.CV.DAL.Utils;

namespace MU.CV.BLL.Domains.JobExperience;

public class JobExperienceWriteService(IBaseRepository<JobExperienceDAL> repo, IUnitOfWork uow)
    : BaseDtoWrite<JobExperienceDAL, JobExperienceDto>(repo, uow);

public class JobExperienceReadService(IProjectorRepository<JobExperienceDAL> repo)
    : BaseDtoRead<JobExperienceDAL, JobExperienceDto>(repo, dal => new JobExperienceDto(
        dal.Id,
        dal.CvId,
        dal.PositionDescription,
        dal.LengthOfWorkFrom,
        dal.LengthOfWorkTo,
        dal.PositionTitle,
        dal.JobCompanyName,
        dal.JobCompanyLogo,
        dal.IsIT,
        dal.IsFreelance));

public static class JobExperienceExtensions
{
    public static IServiceCollection AddJobExperienceServices(this IServiceCollection services)
    {
        services.AddScoped<IDtoWrite<JobExperienceDto>, JobExperienceWriteService>();
        services.AddScoped<IDtoRead<JobExperienceDto>, JobExperienceReadService>();
        
        return services;
    }
}

public record JobExperienceDto(
    Guid Id,
    Guid CvId,
    string PositionDescription,
    DateOnly LengthOfWorkFrom,
    DateOnly? LengthOfWorkTo,
    string PositionTitle,
    string JobCompanyName,
    string JobCompanyLogo,
    bool IsIT,
    bool IsFreelance
) : ValueBaseDtoEntity<JobExperienceDAL>(Id)
{
    public JobExperienceDto() : this(Guid.Empty, Guid.Empty, string.Empty, default, null, string.Empty, string.Empty, string.Empty, true, false) { }

    public override JobExperienceDAL ToDbEntity()
    {
        return new JobExperienceDAL()
        {
            Id = Id,
            CvId = CvId,
            PositionDescription = PositionDescription,
            LengthOfWorkFrom = LengthOfWorkFrom,
            LengthOfWorkTo = LengthOfWorkTo,
            PositionTitle = PositionTitle,
            JobCompanyName = JobCompanyName,
            JobCompanyLogo = JobCompanyLogo,
            IsIT = IsIT,
            IsFreelance = IsFreelance
        };
    }
}


