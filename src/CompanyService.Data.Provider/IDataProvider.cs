using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.CompanyService.Data.Provider
{
    public interface IDataProvider
    {
        DbSet<DbPosition> GetPositions();
        DbSet<DbCompany> GetCompanies();
        DbSet<DbDepartment> GetDepartments();
        DbSet<DbCompanyUser> GetCompaniesUsers();
    }
}
