using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Business.Commands.CompanyUser.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.CompanyService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class CompanyUserController : ControllerBase
  {
    [HttpPatch("edit")]
    public async Task<OperationResultResponse<bool>> EditAsync(
      [FromServices] IEditCompanyUserCommand command,
      [FromBody] EditCompanyUserRequest request)
    {
      return await command.ExecuteAsync(request);
    }
  }
}
