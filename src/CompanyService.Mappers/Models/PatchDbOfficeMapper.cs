using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
  public class PatchDbOfficeMapper : IPatchDbOfficeMapper
  {
    public JsonPatchDocument<DbOffice> Map(JsonPatchDocument<EditOfficeRequest> request)
    {
      if (request == null)
      {
        return null;
      }

      JsonPatchDocument<DbOffice> patchDbNews = new JsonPatchDocument<DbOffice>();

      foreach (Operation<EditOfficeRequest> item in request.Operations)
      {
        patchDbNews.Operations.Add(new Operation<DbOffice>(
          item.op,
          item.path,
          item.from,
          item.value?.ToString().Trim() == string.Empty ? null : item.value?.ToString().Trim()));
      }

      return patchDbNews;
    }
  }
}
