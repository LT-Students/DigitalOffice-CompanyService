using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Database;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.CompanyService.Data.Provider
{
    public interface IDataProvider : IBaseDataProvider
    {
        DbSet<DbPosition> Positions { get; set; }
        DbSet<DbDepartment> Departments { get; set; }
        DbSet<DbDepartmentUser> DepartmentsUsers { get; set; }
    }
}