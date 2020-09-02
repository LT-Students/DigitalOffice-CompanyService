using FluentValidation;
using LT.DigitalOffice.CompanyService.Commands.Interfaces;
using LT.DigitalOffice.CompanyService.Database.Entities;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Repositories.Interfaces;
using LT.DigitalOffice.CompanyService.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.CompanyService.Commands
{
    public class AddCompanyCommand : IAddCompanyCommand
    {
        private readonly IValidator<AddCompanyRequest> validator;
        private readonly IMapper<AddCompanyRequest, DbCompany> mapper;
        private readonly ICompanyRepository repository;

        public AddCompanyCommand(
            [FromServices] IValidator<AddCompanyRequest> validator,
            [FromServices] IMapper<AddCompanyRequest, DbCompany> mapper,
            [FromServices] ICompanyRepository repository)
        {
            this.validator = validator;
            this.repository = repository;
            this.mapper = mapper;
        }

        public Guid Execute(AddCompanyRequest request)
        {
            validator.ValidateAndThrow(request);

            var dbCompany = mapper.Map(request);

            return repository.AddCompany(dbCompany);
        }
    }
}

