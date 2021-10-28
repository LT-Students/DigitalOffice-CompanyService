using System;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Office.Interface
{
  [AutoInject]
  public interface IEditOfficeCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(Guid officeId, JsonPatchDocument<EditOfficeRequest> request);
  }
}
