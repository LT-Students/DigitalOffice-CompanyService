using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.CompanyService.Business.Commands.ContractSubject.Interfaces
{
  [AutoInject]
  public interface IGetContractSubjectsCommand
  {
    Task<OperationResultResponse<List<ContractSubjectInfo>>> ExecuteAsync(Guid companyId);
  }
}
