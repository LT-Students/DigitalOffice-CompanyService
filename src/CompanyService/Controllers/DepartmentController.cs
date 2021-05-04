using LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.CompanyService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        [HttpPost("create")]
        public Guid Create(
            [FromServices] ICreateDepartmentCommand command,
            [FromBody] NewDepartmentRequest department)
        {
            return command.Execute(department);
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
