using LT.DigitalOffice.CompanyService.Commands.Interfaces;
using LT.DigitalOffice.CompanyService.Database.Entities;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models;
using LT.DigitalOffice.CompanyService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Commands
{
    public class GetCompaniesListCommand : IGetCompaniesListCommand
    {
        private readonly ICompanyRepository repository;
        private readonly IMapper<DbCompany, Company> mapper;

        public GetCompaniesListCommand(
            [FromServices] ICompanyRepository repository,
            [FromServices] IMapper<DbCompany, Company> mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public List<Company> Execute()
        {
            var dbCompanies = repository.GetCompaniesList();

            return dbCompanies.Select(dbCompany => mapper.Map(dbCompany)).ToList();
        }
    }
}

