using System;
using LT.DigitalOffice.CompanyService.Business.Commands.Office.Interface;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.CompanyService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class OfficeController : ControllerBase
  {
    [HttpPost("create")]
    public OperationResultResponse<Guid> Create(
      [FromServices] ICreateOfficeCommand command,
      [FromBody] CreateOfficeRequest request)
    {
      return command.Execute(request);
    }

    [HttpGet("find")]
    public FindResultResponse<OfficeInfo> Find(
      [FromServices] IFindOfficesCommand command,
      [FromQuery] BaseFindFilter filter)
    {
      return command.Execute(filter);
    }

    [HttpPatch("edit")]
    public OperationResultResponse<bool> Edit(
      [FromServices] IEditOfficeCommand command,
      [FromQuery] Guid officeId,
      [FromBody] JsonPatchDocument<EditOfficeRequest> request)
    {
      return command.Execute(officeId, request);
    }
  }
}
