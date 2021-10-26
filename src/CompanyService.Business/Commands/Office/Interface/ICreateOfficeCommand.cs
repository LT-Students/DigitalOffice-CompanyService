using System;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Office.Interface
{
  [AutoInject]
  public interface ICreateOfficeCommand
  {
    Task<OperationResultResponse<Guid>> ExecuteAsync(CreateOfficeRequest request);
  }
}
