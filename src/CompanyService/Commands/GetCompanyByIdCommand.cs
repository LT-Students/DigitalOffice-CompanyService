using LT.DigitalOffice.CompanyService.Commands.Interfaces;
using LT.DigitalOffice.CompanyService.Database.Entities;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models;
using LT.DigitalOffice.CompanyService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.CompanyService.Commands
{
    public class GetCompanyByIdCommand : IGetCompanyByIdCommand
    {
        private readonly ICompanyRepository repository;
        private readonly IMapper<DbCompany, Company> mapper;

        public GetCompanyByIdCommand(
            [FromServices] ICompanyRepository repository,
            [FromServices] IMapper<DbCompany, Company> mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public Company Execute(Guid companyId)
        {
            var dbCompany = repository.GetCompanyById(companyId);

            return mapper.Map(dbCompany);
        }
    }
}

