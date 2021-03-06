﻿using LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.CompanyService.Validation.Department.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Department
{
    public class CreateDepartmentCommand : ICreateDepartmentCommand
    {
        private readonly IDepartmentRepository _repository;
        private readonly ICreateDepartmentRequestValidator _validator;
        private readonly ICompanyRepository _companyRepository;
        private readonly IDbDepartmentMapper _mapper;
        private readonly IAccessValidator _accessValidator;

        public CreateDepartmentCommand(
            IDepartmentRepository repository,
            ICompanyRepository companyRepository,
            ICreateDepartmentRequestValidator validator,
            IDbDepartmentMapper mapper,
            IAccessValidator accessValidator)
        {
            _repository = repository;
            _companyRepository = companyRepository;
            _validator = validator;
            _mapper = mapper;
            _accessValidator = accessValidator;
        }

        public OperationResultResponse<Guid> Execute(CreateDepartmentRequest request)
        {
            if (!(_accessValidator.IsAdmin() || _accessValidator.HasRights(Rights.AddEditRemoveDepartments)))
            {
                throw new ForbiddenException("Not enough rights.");
            }

            _validator.ValidateAndThrowCustom(request);

            var company = _companyRepository.Get();

            if (company == null)
            {
                return new OperationResultResponse<Guid>
                {
                    Status = OperationResultStatusType.Failed,
                    Errors = new List<string> () { "Company does not exist" }
                };
            }

            return new OperationResultResponse<Guid>
            {
                Status = OperationResultStatusType.FullSuccess,
                Body = _repository.CreateDepartment(_mapper.Map(request, company.Id))
            };
        }
    }
}
