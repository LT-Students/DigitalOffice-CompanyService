using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.CompanyService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        [HttpPost("create")]
        public OperationResultResponse<Guid> Create(
            [FromBody] CreateCompanyRequest request,
            [FromServices] ICreateCompanyCommand command)
        {
            return command.Execute(request);
        }

        [HttpGet("get")]
        public CompanyResponse Get(
            [FromServices] IGetCompanyCommand command)
        {
            return command.Execute();
        }
    }
}
