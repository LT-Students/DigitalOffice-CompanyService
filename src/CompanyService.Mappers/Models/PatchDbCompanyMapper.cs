using System;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
  public class PatchDbCompanyMapper : IPatchDbCompanyMapper
  {
    public JsonPatchDocument<DbCompany> Map(JsonPatchDocument<EditCompanyRequest> request)
    {
      if (request == null)
      {
        return null;
      }

      JsonPatchDocument<DbCompany> result = new JsonPatchDocument<DbCompany>();

      foreach (Operation<EditCompanyRequest> item in request.Operations)
      {
        if (item.path.EndsWith(nameof(EditCompanyRequest.Logo), StringComparison.OrdinalIgnoreCase))
        {
          AddImageRequest image = JsonConvert.DeserializeObject<AddImageRequest>(item.value.ToString());
          result.Operations.Add(new Operation<DbCompany>(item.op, nameof(DbCompany.LogoContent), item.from, image.Content));
          result.Operations.Add(new Operation<DbCompany>(item.op, nameof(DbCompany.LogoExtension), item.from, image.Extension));

          continue;
        }

        result.Operations.Add(new Operation<DbCompany>(item.op, item.path, item.from, item.value));
      }

      return result;
    }
  }
}
