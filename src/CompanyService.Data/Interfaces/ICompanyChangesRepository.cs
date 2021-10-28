using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
  [AutoInject]
  public interface ICompanyChangesRepository
  {
    Task CreateAsync(Guid companyId, Guid? changedBy, string changes);
  }
}
