using LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        [HttpPost("create")]
        public OperationResultResponse<Guid> Create(
            [FromServices] ICreateDepartmentCommand command,
            [FromBody] CreateDepartmentRequest department)
        {
            return command.Execute(department);
        }

        [HttpGet("get")]
        public async Task<OperationResultResponse<DepartmentResponse>> Get(
            [FromServices] IGetDepartmentCommand command,
            [FromQuery] GetDepartmentFilter filter)
        {
            return await command.Execute(filter);
        }

        [HttpGet("find")]
        public async Task<FindResultResponse<DepartmentInfo>> Find(
            [FromServices] IFindDepartmentsCommand command,
            [FromQuery] int skipCount,
            [FromQuery] int takeCount,
            [FromQuery] bool includeDeactivated = false)
        {
            return await command.Execute(skipCount, takeCount, includeDeactivated);
        }

        [HttpPatch("edit")]
        public OperationResultResponse<bool> Edit(
            [FromServices] IEditDepartmentCommand command,
            [FromQuery] Guid departmentId,
            [FromBody] JsonPatchDocument<EditDepartmentRequest> request)
        {
            return command.Execute(departmentId, request);
        }
    }
}
