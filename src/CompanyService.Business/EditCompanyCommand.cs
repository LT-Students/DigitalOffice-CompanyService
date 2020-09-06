using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.CompanyService.Data.Interfaces;

namespace LT.DigitalOffice.CompanyService.Business
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

