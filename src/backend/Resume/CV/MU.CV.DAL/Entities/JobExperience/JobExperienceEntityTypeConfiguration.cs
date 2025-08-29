using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MU.CV.DAL.Entities.JobExperience;

public class JobExperienceEntityTypeConfiguration : IEntityTypeConfiguration<JobExperienceDAL>
{
    public void Configure(EntityTypeBuilder<JobExperienceDAL> builder)
    {
        builder.ToTable("job_experience");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.CvId).IsRequired();
        builder.Property(e => e.PositionDescription).IsRequired(false);
        builder.Property(e => e.LengthOfWorkFrom).IsRequired();
        builder.Property(e => e.LengthOfWorkTo).IsRequired(false);
        builder.Property(e => e.PositionTitle).IsRequired().HasMaxLength(256);
        builder.Property(e => e.JobCompanyName).IsRequired().HasMaxLength(256);
        builder.Property(e => e.JobCompanyLogo).IsRequired(false);
        builder.Property(e => e.IsIT).HasDefaultValue(true);
        builder.Property(e => e.IsFreelance).HasDefaultValue(false);
    }
}


