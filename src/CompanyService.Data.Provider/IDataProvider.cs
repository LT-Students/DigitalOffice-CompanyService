using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Database;
using LT.DigitalOffice.Kernel.Enums;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.CompanyService.Data.Provider
{
    [AutoInject(InjectType.Scoped)]
    public interface IDataProvider : IBaseDataProvider
    {
        DbSet<DbPosition> Positions { get; set; }
        DbSet<DbDepartment> Departments { get; set; }
        DbSet<DbDepartmentUser> DepartmentUsers { get; set; }
        DbSet<DbPositionUser> PositionUsers { get; set; }
        DbSet<DbCompany> Companies { get; set; }
        DbSet<DbOffice> Offices { get; set; }
    }
}