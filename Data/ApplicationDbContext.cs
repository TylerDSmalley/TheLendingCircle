using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace TheLendingCircle.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
    
    public override DbSet<IdentityUser> Users { get; set; }

    //Probably can be removed once we migrate to hosted DB
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //TODO: Add SqlServer data source here
            optionsBuilder.UseSqlite(@"Data source=TheLendingCircleDb.sqlite");
        }
}
