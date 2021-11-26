using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
  [AutoInject]
  public interface ICompanyRepository
  {
    Task CreateAsync(DbCompany company);

    Task<DbCompany> GetAsync();

    Task EditAsync(JsonPatchDocument<DbCompany> request);

    Task<List<DbCompany>> GetAsync(IGetCompaniesRequest request);
  }
}
