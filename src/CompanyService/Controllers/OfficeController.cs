using LT.DigitalOffice.CompanyService.Business.Commands.Office.Interface;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace LT.DigitalOffice.CompanyService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OfficeController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OfficeController(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("create")]
        public OperationResultResponse<Guid> Create(
            [FromServices] ICreateOfficeCommand command,
            [FromBody] CreateOfficeRequest request)
        {
            _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

            return command.Execute(request);
        }

        [HttpGet("find")]
        public OfficesResponse Find(
            [FromServices] IFindOfficesCommand command,
            [FromQuery] int skipCount,
            [FromQuery] int takeCount)
        {
            return command.Execute(skipCount, takeCount);
        }
    }
}
