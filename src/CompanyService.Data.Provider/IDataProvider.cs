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
    DbSet<DbCompany> Companies { get; set; }
    DbSet<DbCompanyChanges> CompanyChanges { get; set; }
    DbSet<DbCompanyUser> CompaniesUsers { get; set; }
    DbSet<DbContractSubject> ContractSubjects { get; set; }
  }
}
