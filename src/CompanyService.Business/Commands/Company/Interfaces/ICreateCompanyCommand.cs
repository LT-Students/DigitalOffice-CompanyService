using System;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces
{
  [AutoInject]
  public interface ICreateCompanyCommand
  {
    Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateCompanyRequest request);
  }
}
