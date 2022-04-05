using System;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company;
using LT.DigitalOffice.Kernel.ImageSupport.Helpers.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;

namespace LT.DigitalOffice.CompanyService.Mappers.Patch
{
  public class PatchDbCompanyMapper : IPatchDbCompanyMapper
  {
    private readonly IImageResizeHelper _imageResizeHelper;

    public PatchDbCompanyMapper(
      IImageResizeHelper imageResizeHelper)
    {
      _imageResizeHelper = imageResizeHelper;
    }

    public async Task<JsonPatchDocument<DbCompany>> MapAsync(JsonPatchDocument<EditCompanyRequest> request)
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
          ImageConsist image = JsonConvert.DeserializeObject<ImageConsist>(item.value.ToString());
          (bool isSuccess, string content, string extension) resizeResults = image is null
            ? default
            : await _imageResizeHelper.ResizeAsync(image.Content, image.Extension);

          result.Operations.Add(new Operation<DbCompany>(
            item.op,
            nameof(DbCompany.LogoContent),
            item.from,
            resizeResults.isSuccess ? resizeResults.content : null));

          result.Operations.Add(new Operation<DbCompany>(
            item.op,
            nameof(DbCompany.LogoExtension),
            item.from,
            resizeResults.isSuccess ? resizeResults.extension : null));

          continue;
        }

        result.Operations.Add(new Operation<DbCompany>(item.op, item.path, item.from, item.value));
      }

      return result;
    }
  }
}
