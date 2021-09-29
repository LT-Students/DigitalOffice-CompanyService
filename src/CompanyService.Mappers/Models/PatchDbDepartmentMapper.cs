using System;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
  public class PatchDbDepartmentMapper : IPatchDbDepartmentMapper
  {
    public JsonPatchDocument<DbDepartment> Map(JsonPatchDocument<EditDepartmentRequest> request)
    {
      if (request == null)
      {
        throw new ArgumentNullException(nameof(request));
      }

      var result = new JsonPatchDocument<DbDepartment>();

      foreach (var item in request.Operations)
      {
        if (item.path.EndsWith(nameof(EditDepartmentRequest.DirectorId), StringComparison.OrdinalIgnoreCase))
        {
          continue;
        }

        result.Operations.Add(new Operation<DbDepartment>(item.op, item.path, item.from, item.value));
      }

      return result;
    }
  }
}
