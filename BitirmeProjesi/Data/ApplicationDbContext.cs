using BitirmeProjesi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BitirmeProjesi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<BitirmeProjesi.Models.Request>? Request { get; set; }
        public DbSet<BitirmeProjesi.Models.Course>? Course { get; set; }
        public DbSet<BitirmeProjesi.Models.EmployeeCourses>? EmployeeCourses { get; set; }

    }
}