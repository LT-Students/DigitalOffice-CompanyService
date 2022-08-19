using System;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Business.Commands.ContractSubject.Interfaces
{
  [AutoInject]
  public interface IEditContractSubjectCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(Guid contractSubjectId, JsonPatchDocument<EditContractSubjectRequest> request);
  }
}
