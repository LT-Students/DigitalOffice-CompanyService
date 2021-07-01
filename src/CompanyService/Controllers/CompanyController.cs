using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
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
    public class CompanyController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompanyController(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("create")]
        public OperationResultResponse<Guid> Create(
            [FromBody] CreateCompanyRequest request,
            [FromServices] ICreateCompanyCommand command)
        {
            _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

            return command.Execute(request);
        }

        [HttpGet("get")]
        public CompanyInfo Get(
            [FromServices] IGetCompanyCommand command)
        {
            return command.Execute();
        }
    }
}
