using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistence;

public class CleanDbContextDesignTimeFactory : IDesignTimeDbContextFactory<CleanDbContext>
{
    public CleanDbContext CreateDbContext(string[] args)
    {
        var optionBuilder = new DbContextOptionsBuilder<CleanDbContext>().UseSqlServer();

        return new CleanDbContext(optionBuilder.Options);
    }
}
