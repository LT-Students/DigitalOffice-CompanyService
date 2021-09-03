using LT.DigitalOffice.CompanyService.Business.Commands.Office.Interface;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Mvc;
using System;

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
            [FromQuery] int skipCount,
            [FromQuery] int takeCount,
            [FromQuery] bool? includeDeactivated)
        {
            return command.Execute(skipCount, takeCount, includeDeactivated);
        }
    }
}
