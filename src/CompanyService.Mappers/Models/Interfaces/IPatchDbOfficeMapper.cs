using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces
{
  [AutoInject]
  public interface IPatchDbOfficeMapper
  {
    JsonPatchDocument<DbOffice> Map(JsonPatchDocument<EditOfficeRequest> request);
  }
}
