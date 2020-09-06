using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef
{
    /// <summary>
    /// A class that defines the tables and its properties in the database of CompanyService.
    /// </summary>
    public class CompanyServiceDbContext : IDataProvider
    {
        public DbSet<DbPosition> Positions { get; set; }
        public DbSet<DbCompany> Companies { get; set; }
        public DbSet<DbDepartment> Departments { get; set; }
        public DbSet<DbCompanyUser> CompaniesUsers { get; set; }
    }
}