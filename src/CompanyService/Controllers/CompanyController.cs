using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        [HttpGet("getCompanyById")]
        public Company GetCompanyById([FromServices] IGetCompanyByIdCommand command, [FromQuery] Guid companyId)
        {
            return command.Execute(companyId);
        }

        [HttpGet("getCompaniesList")]
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
    }
}