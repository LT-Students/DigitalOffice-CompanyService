using FluentValidation;
using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Business
{
    /// <inheritdoc cref="IAddCompanyCommand"/>
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
            validator.ValidateAndThrowCustom(request);

            var dbCompany = mapper.Map(request);

            return repository.AddCompany(dbCompany);
        }
    }
}

