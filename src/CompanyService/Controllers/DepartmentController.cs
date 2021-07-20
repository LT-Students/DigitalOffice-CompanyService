using LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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

            if (result.Status == OperationResultStatusType.Conflict)
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;

                return result;
            }

            if (result.Status != OperationResultStatusType.Failed)
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
            }

            return result;
        }

        [HttpGet("get")]
        public OperationResultResponse<DepartmentResponse> Get(
            [FromServices] IGetDepartmentCommand command,
            [FromQuery] GetDepartmentFilter filter)
        {
            return command.Execute(filter);
        }

        [HttpGet("find")]
        public FindDepartmentsResponse Find(
            [FromServices] IFindDepartmentsCommand command,
            [FromQuery] int skipCount,
            [FromQuery] int takeCount,
            [FromQuery] bool includeDeactivated = false)
        {
            return command.Execute(skipCount, takeCount, includeDeactivated);
        }

        [HttpPatch("edit")]
        public OperationResultResponse<bool> Edit(
            [FromServices] IEditDepartmentCommand command,
            [FromQuery] Guid departmentId,
            [FromBody] JsonPatchDocument<EditDepartmentRequest> request)
        {
            var result = command.Execute(departmentId, request);

            if (result.Status == OperationResultStatusType.Conflict)
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
            }

            return result;
        }
    }
}
