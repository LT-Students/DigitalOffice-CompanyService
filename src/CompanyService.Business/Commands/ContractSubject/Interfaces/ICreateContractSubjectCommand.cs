using System;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.CompanyService.Business.Commands.ContractSubject.Interfaces
{
  [AutoInject]
  public interface ICreateContractSubjectCommand
  {
    Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateContractSubjectRequest request);
  }
}
