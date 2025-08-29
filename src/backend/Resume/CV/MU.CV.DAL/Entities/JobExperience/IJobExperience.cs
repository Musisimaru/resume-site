namespace MU.CV.DAL.Entities.JobExperience;

public interface IJobExperience
{
    string PositionDescription { get; set; }

    DateOnly LengthOfWorkFrom { get; set; }
    DateOnly? LengthOfWorkTo { get; set; }

    TimeSpan PositionExpCount { get; }

    string PositionTitle { get; set; }

    string JobCompanyName { get; set; }
    TimeSpan JobExpCount { get; }
    string JobCompanyLogo { get; set; }

    bool IsIT { get; set; }
    bool IsFreelance { get; set; }
}


