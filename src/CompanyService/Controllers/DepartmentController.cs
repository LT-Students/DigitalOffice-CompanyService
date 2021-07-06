using LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace LT.DigitalOffice.CompanyService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DepartmentController(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("create")]
        public OperationResultResponse<Guid> Create(
            [FromServices] ICreateDepartmentCommand command,
            [FromBody] CreateDepartmentRequest department)
        {
            var result = command.Execute(department);

            if (result.Status != OperationResultStatusType.Failed)
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
            }

            return result;
        }

        [HttpGet("get")]
        public DepartmentInfo Get(
            [FromServices] IGetDepartmentByIdCommand command,
            [FromQuery] Guid id)
        {
            return command.Execute(id);
        }

        [HttpGet("find")]
        public DepartmentsResponse Find(
            [FromServices] IFindDepartmentsCommand command)
        {
            return command.Execute();
        }
    }
}
