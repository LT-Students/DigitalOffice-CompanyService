using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Mappers.Patch.Interfaces
{
  [AutoInject]
  public interface IPatchContractSubjectMapper
  {
    JsonPatchDocument<DbContractSubject> Map(JsonPatchDocument<EditContractSubjectRequest> request);
  }
}
