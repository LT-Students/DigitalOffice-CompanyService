using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Database;
using LT.DigitalOffice.Kernel.Enums;

namespace LT.DigitalOffice.CompanyService.Data.Provider
{
  [AutoInject(InjectType.Scoped)]
  public interface IDataProvider : IBaseDataProvider
  {
    IQueryable<DbCompany> Companies { get; }
    IQueryable<DbCompanyChanges> CompanyChanges { get; }
    void AddCompanyOrChanges<T>(T item);
    Task<DbCompany> GetCompanyAsync();
    Task<bool> AnyCompanyAsync();
  }
}
