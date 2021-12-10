using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Mappers.Patch.Interfaces
{
  [AutoInject]
  public interface IPatchCompanyUserMapper
  {
    JsonPatchDocument<DbCompanyUser> Map(JsonPatchDocument<EditCompanyUserRequest> request);
  }
}
