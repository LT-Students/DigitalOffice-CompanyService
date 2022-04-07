using System;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.CompanyService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class CompanyController : ControllerBase
  {
    [HttpPost]
    public async Task<OperationResultResponse<Guid?>> Create(
      [FromServices] ICreateCompanyCommand command,
      [FromBody] CreateCompanyRequest request)
    {
      return await command.ExecuteAsync(request);
    }

    [HttpGet("{companyId}")]
    public async Task<OperationResultResponse<CompanyResponse>> GetAsync(
      [FromServices] IGetCompanyCommand command,
      [FromRoute] Guid companyId,
      [FromQuery] GetCompanyFilter filter)
    {
      return await command.ExecuteAsync(companyId, filter);
    }

    [HttpPatch("{companyId}")]
    public async Task<OperationResultResponse<bool>> EditAsync(
      [FromServices] IEditCompanyCommand command,
      [FromRoute] Guid companyId,
      [FromBody] JsonPatchDocument<EditCompanyRequest> request)
    {
      return await command.ExecuteAsync(companyId, request);
    }
  }
}
