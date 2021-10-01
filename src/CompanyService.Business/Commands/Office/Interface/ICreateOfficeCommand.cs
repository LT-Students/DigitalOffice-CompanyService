using System;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Office.Interface
{
  [AutoInject]
  public interface ICreateOfficeCommand
  {
    OperationResultResponse<Guid> Execute(CreateOfficeRequest request);
  }
}
