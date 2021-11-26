using LT.DigitalOffice.CompanyService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.CompanyService.Mappers.Patch
{
  public class PatchCompanyUserMapper : IPatchCompanyUserMapper
  {
    public JsonPatchDocument<DbCompanyUser> Map(JsonPatchDocument<EditCompanyUserRequest> request)
    {
      if (request is null)
      {
        return null;
      }

      JsonPatchDocument<DbCompanyUser> patch = new JsonPatchDocument<DbCompanyUser>();

      foreach (Operation<EditCompanyUserRequest> item in request.Operations)
      {
        patch.Operations.Add(new Operation<DbCompanyUser>(item.op, item.path, item.from, item.value));
      }

      return patch;
    }
  }
}

