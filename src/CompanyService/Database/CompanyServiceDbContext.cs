using LT.DigitalOffice.CompanyService.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LT.DigitalOffice.CompanyService.Database
{
    /// <summary>
    /// A class that defines the tables and its properties in the database of CompanyService.
    /// </summary>
    public class CompanyServiceDbContext : DbContext
    {
        public CompanyServiceDbContext (DbContextOptions<CompanyServiceDbContext> options)
            :base(options)
        {
        }

        public DbSet<DbPosition> Positions { get; set; }
        public DbSet<DbCompany> Companies { get; set; }
        public DbSet<DbDepartment> Departments { get; set; }
        public DbSet<DbCompanyUser> CompaniesUsers { get; set; }

        // Fluent API is written here.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}