using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MU.CV.DAL.DataContext;

public class CVDbContext(DbContextOptions options) : DbContext(options)
{
    public const double COMMAND_TIMEOUT__MINUTES = 5;
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // регистр не учитывается
        // диакритики учитываются
        builder.UseCollation("cyrillic_general_ci_as");
        builder.ApplyConfigurationsFromAssembly(
            assembly: typeof(CVDbContext).Assembly
        );
    }
    
    
    public class CVDBContextFactory : IDesignTimeDbContextFactory<CVDbContext>
    {
        public CVDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CVDbContext>();
            optionsBuilder.UseNpgsql(opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(COMMAND_TIMEOUT__MINUTES).TotalSeconds));

            return new CVDbContext(optionsBuilder.Options);
        }
    }
}