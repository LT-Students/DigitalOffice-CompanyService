using System;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.CompanyService.Business.Commands.CompanyUser.Interfaces
{
  [AutoInject]
  public interface ICreateCompanyUserCommand
  {
    Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateCompanyUserRequest request);
  }
}
