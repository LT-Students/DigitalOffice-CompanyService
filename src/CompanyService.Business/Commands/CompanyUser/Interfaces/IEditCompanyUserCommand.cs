using System;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Business.Commands.CompanyUser.Interfaces
{
  [AutoInject]
  public interface IEditCompanyUserCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(
      Guid userId,
      JsonPatchDocument<EditCompanyUserRequest> request);
  }
}
