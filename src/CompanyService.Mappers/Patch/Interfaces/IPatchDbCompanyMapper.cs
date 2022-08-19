using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Mappers.Patch.Interfaces
{
  [AutoInject]
  public interface IPatchDbCompanyMapper
  {
    Task<JsonPatchDocument<DbCompany>> MapAsync(JsonPatchDocument<EditCompanyRequest> request);
  }
}
