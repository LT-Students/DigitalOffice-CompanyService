using System;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces
{
  [AutoInject]
  public interface IGetCompanyCommand
  {
    Task<OperationResultResponse<CompanyInfo>> ExecuteAsync(Guid companyId, GetCompanyFilter filter);
  }
}
