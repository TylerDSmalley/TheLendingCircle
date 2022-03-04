using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TheLendingCircle.Models;

namespace TheLendingCircle.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
    
    public DbSet<Item> Items { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public override DbSet<ApplicationUser> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

    //Used for local DB
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     {
    //         //TODO: Add SqlServer data source here
    //         optionsBuilder.UseSqlServer(@"Data source=LendingCircleContext");
    //     }

    //Probably can be removed once we migrate to hosted DB
    public DbSet<TheLendingCircle.Models.Request> Request { get; set; }

    //Probably can be removed once we migrate to hosted DB
    public DbSet<TheLendingCircle.Models.Review> Review { get; set; }
}
