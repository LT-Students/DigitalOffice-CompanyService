using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.CompanyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        [HttpPost("create")]
        public Guid AddDepartment(
            [FromServices] ICreateDepartmentCommand command,
            [FromBody] NewDepartmentRequest department)
        {
            return command.Execute(department);
        }
    }
}
