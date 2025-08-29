using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MU.CV.DAL.Entities.Cv;

public class CvEntityTypeConfiguration : IEntityTypeConfiguration<CvDAL>
{
    public void Configure(EntityTypeBuilder<CvDAL> builder)
    {
        builder.ToTable("cv");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.OwnerId).IsRequired();
        builder.Property(e => e.OwnerFullName).IsRequired().HasMaxLength(256);
        builder.Property(e => e.Title).IsRequired().HasMaxLength(256);
        builder.Property(e => e.About).IsRequired(false);

        builder.HasMany(e => e.JobExperiences)
            .WithOne(e => e.Cv)
            .HasForeignKey(e => e.CvId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}


