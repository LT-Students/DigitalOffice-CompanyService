using System;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Business.Commands.CompanyUser.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.CompanyService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class CompanyUserController : ControllerBase
  {
    [HttpPatch("{userId}")]
    public async Task<OperationResultResponse<bool>> EditAsync(
      [FromServices] IEditCompanyUserCommand command,
      [FromRoute] Guid userId,
      [FromBody] JsonPatchDocument<EditCompanyUserRequest> request)
    {
      return await command.ExecuteAsync(userId, request);
    }

    [HttpPost]
    public async Task<OperationResultResponse<Guid?>> CreateAsync(
      [FromServices] ICreateCompanyUserCommand command,
      [FromBody] CreateCompanyUserRequest request)
    {
      return await command.ExecuteAsync(request);
    }
  }
}
