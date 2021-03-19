using FluentValidation;
using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.CompanyService.Business
{
    public class CreateDepartmentCommand : ICreateDepartmentCommand
    {
        private readonly IDepartmentRepository repository;
        private readonly IValidator<NewDepartmentRequest> validator;
        private readonly IMapper<NewDepartmentRequest, DbDepartment> mapper;
        private readonly IAccessValidator accessValidator;

        public CreateDepartmentCommand(
            [FromServices] IDepartmentRepository repository,
            [FromServices] IValidator<NewDepartmentRequest> validator,
            [FromServices] IMapper<NewDepartmentRequest, DbDepartment> mapper,
            [FromServices] IAccessValidator accessValidator)
        {
            this.repository = repository;
            this.validator = validator;
            this.mapper = mapper;
            this.accessValidator = accessValidator;
        }

        public Guid Execute(NewDepartmentRequest request)
        {
            const int rightId = 4;

            if (!(accessValidator.IsAdmin() || accessValidator.HasRights(rightId)))
            {
                throw new ForbiddenException("Not enough rights.");
            }

            validator.ValidateAndThrowCustom(request);

            return repository.CreateDepartment(mapper.Map(request));
        }
    }
}
