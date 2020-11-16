using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.CompanyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        [HttpPost("addDepartment")]
        public Guid AddDepartment(
            [FromServices] IAddDepartmentCommand command,
            [FromBody] DepartmentRequest department)
        {
            return command.Execute(department);
        }
    }
}
