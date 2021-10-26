using System;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.CompanyService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class CompanyController : ControllerBase
  {
    [HttpPost("create")]
    public async Task<OperationResultResponse<Guid>> Create(
      [FromServices] ICreateCompanyCommand command,
      [FromBody] CreateCompanyRequest request)
    {
      return await command.ExecuteAsync(request);
    }

    [HttpGet("get")]
    public OperationResultResponse<CompanyInfo> Get(
      [FromServices] IGetCompanyCommand command,
      [FromQuery] GetCompanyFilter filter)
    {
      return command.Execute(filter);
    }

    [HttpPatch("edit")]
    public async Task<OperationResultResponse<bool>> EditAsync(
      [FromServices] IEditCompanyCommand command,
      [FromBody] JsonPatchDocument<EditCompanyRequest> request)
    {
      return await command.ExecuteAsync(request);
    }
  }
}
