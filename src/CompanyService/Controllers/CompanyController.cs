using LT.DigitalOffice.CompanyService.Commands.Interfaces;
using LT.DigitalOffice.CompanyService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        [HttpPost("getCompanyById")]
        public Company GetCompanyById([FromServices] IGetCompanyByIdCommand command, [FromQuery] Guid companyId)
        {
            return command.Execute(companyId);
        }

        [HttpPost("getCompaniesList")]
        public List<Company> GetCompaniesList([FromServices] IGetCompaniesListCommand command)
        {
            return command.Execute();
        }

        [HttpPost("addCompany")]
        public Guid AddCompany([FromServices] IAddCompanyCommand command, [FromBody] AddCompanyRequest request)
        {
            return command.Execute(request);
        }

        [HttpPost("changeCompany")]
        public bool ChangeCompany([FromServices] IEditCompanyCommand command, [FromBody] EditCompanyRequest request)
        {
            return command.Execute(request);
        }

        [HttpGet("getPositionById")]
        public Position GetPositionById([FromServices] IGetPositionByIdCommand command, [FromQuery] Guid positionId)
        {
            return command.Execute(positionId);
        }

        [HttpGet("getPositionsList")]
        public List<Position> GetPositionsList([FromServices] IGetPositionsListCommand command)
        {
            return command.Execute();
        }

        [HttpPost("addPosition")]
        public Guid AddPosition([FromServices] IAddPositionCommand command, [FromBody] AddPositionRequest request)
        {
            return command.Execute(request);
        }

        [HttpDelete("disablePositionById")]
        public void DisablePositionById([FromServices] IDisablePositionByIdCommand command, [FromQuery] Guid positionId)
        {
            command.Execute(positionId);
        }

        [HttpPost("editPosition")]
        public bool EditPosition([FromServices] IEditPositionCommand command, [FromBody] EditPositionRequest request)
        {
            return command.Execute(request);
        }
    }
}