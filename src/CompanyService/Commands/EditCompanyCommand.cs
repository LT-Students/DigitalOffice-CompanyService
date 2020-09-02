using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using LT.DigitalOffice.CompanyService.Commands.Interfaces;
using LT.DigitalOffice.CompanyService.Database.Entities;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Repositories.Interfaces;
using LT.DigitalOffice.CompanyService.Models;

namespace LT.DigitalOffice.CompanyService.Commands
{
    public class EditCompanyCommand : IEditCompanyCommand
    {
        private readonly IValidator<EditCompanyRequest> validator;
        private readonly IMapper<EditCompanyRequest, DbCompany> mapper;
        private readonly ICompanyRepository repository;

        public EditCompanyCommand(
            [FromServices] IValidator<EditCompanyRequest> validator,
            [FromServices] IMapper<EditCompanyRequest, DbCompany> mapper,
            [FromServices] ICompanyRepository repository)
        {
            this.validator = validator;
            this.mapper = mapper;
            this.repository = repository;
        }

        public bool Execute(EditCompanyRequest request)
        {
            validator.ValidateAndThrow(request);
            var dbCompany = mapper.Map(request);

            return repository.UpdateCompany(dbCompany);
        }
    }
}

