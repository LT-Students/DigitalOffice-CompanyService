using LT.DigitalOffice.CompanyService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.CompanyService.Mappers.Patch
{
  public class PatchContractSubjectMapper : IPatchContractSubjectMapper
  {
    public JsonPatchDocument<DbContractSubject> Map(JsonPatchDocument<EditContractSubjectRequest> request)
    {
      if (request is null)
      {
        return null;
      }

      JsonPatchDocument<DbContractSubject> patch = new(); 

      foreach (Operation<EditContractSubjectRequest> item in request.Operations)
      {
        patch.Operations.Add(new Operation<DbContractSubject>(item.op, item.path, item.from, item.value));
      }

      return patch;
    }
  }
}
