using System;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces
{
  [AutoInject]
  public interface IGetCompanyCommand
  {
    Task<OperationResultResponse<CompanyResponse>> ExecuteAsync(Guid companyId, GetCompanyFilter filter);
  }
}
