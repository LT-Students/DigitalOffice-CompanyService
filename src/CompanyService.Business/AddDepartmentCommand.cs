﻿using FluentValidation;
using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.CompanyService.Business
{
    public class AddDepartmentCommand : IAddDepartmentCommand
    {
        private readonly IDepartmentRepository repository;
        private readonly IValidator<DepartmentRequest> validator;
        private readonly IMapper<DepartmentRequest, DbDepartment> mapper;
        private readonly IAccessValidator accessValidator;

        public AddDepartmentCommand(
            [FromServices] IDepartmentRepository repository,
            [FromServices] IValidator<DepartmentRequest> validator,
            [FromServices] IMapper<DepartmentRequest, DbDepartment> mapper,
            [FromServices] IAccessValidator accessValidator)
        {
            this.repository = repository;
            this.validator = validator;
            this.mapper = mapper;
            this.accessValidator = accessValidator;
        }

        public Guid Execute(DepartmentRequest request)
        {
            const int rightId = 4;

            if (!(accessValidator.IsAdmin() || accessValidator.HasRights(rightId)))
            {
                throw new Exception("Not enough rights.");
            }

            validator.ValidateAndThrowCustom(request);

            return repository.AddDepartment(mapper.Map(request));
        }
    }
}
