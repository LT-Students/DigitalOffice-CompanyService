using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.CompanyService.Data.Provider
{
    public interface IDataProvider
    {
        DbSet<DbPosition> Positions { get; set; }
        DbSet<DbCompany> Companies { get; set; }
        DbSet<DbDepartment> Departments { get; set; }
        DbSet<DbCompanyUser> CompaniesUsers { get; set; }
    }
}