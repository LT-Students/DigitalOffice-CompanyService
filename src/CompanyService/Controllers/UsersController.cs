using LT.DigitalOffice.CompanyService.Business.Commands.User.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.User;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.CompanyService.Controllers
{
  [ApiController]
  [Route("[Controller]")]
  public class UsersController : ControllerBase
  {
    [HttpPost("create")]
    public OperationResultResponse<bool> Create(
      [FromServices] ICreateDepartmetUsersCommand command,
      [FromBody] CreateDepartmentUsersRequest request)
    {
      return command.Execute(request);
    }
  }
}
