using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.CompanyService.Data.Interfaces;

namespace LT.DigitalOffice.CompanyService.Business
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

