using LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.CompanyService.Validation.Department.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Department
{
    public class CreateDepartmentCommand : ICreateDepartmentCommand
    {
        private readonly IDepartmentRepository _repository;
        private readonly IDepartmentUserRepository _userRepository;
        private readonly ICreateDepartmentRequestValidator _validator;
        private readonly ICompanyRepository _companyRepository;
        private readonly IDbDepartmentMapper _mapper;
        private readonly IAccessValidator _accessValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateDepartmentCommand(
            IDepartmentRepository repository,
            ICompanyRepository companyRepository,
            IDepartmentUserRepository userRepository,
            ICreateDepartmentRequestValidator validator,
            IDbDepartmentMapper mapper,
            IAccessValidator accessValidator,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _validator = validator;
            _mapper = mapper;
            _accessValidator = accessValidator;
            _httpContextAccessor = httpContextAccessor;
        }

        public OperationResultResponse<Guid> Execute(CreateDepartmentRequest request)
        {
            if (!(_accessValidator.IsAdmin() || _accessValidator.HasRights(Rights.AddEditRemoveDepartments)))
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

                return new OperationResultResponse<Guid>
                {
                    Status = OperationResultStatusType.Failed,
                    Errors = new() { "Not enough rights." }
                };
            }

            if (!_validator.ValidateCustom(request, out List<string> errors))
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return new OperationResultResponse<Guid>
                {
                    Status = OperationResultStatusType.Failed,
                    Errors = errors
                };
            }

            OperationResultResponse<Guid> response = new();

            DbCompany company = _companyRepository.Get();

            if (company == null)
            {
                response.Status = OperationResultStatusType.Failed;
                response.Errors.Add("Company does not exist, please create company.");
                return response;
            }

            if (_repository.DoesNameExist(request.Name))
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;

                response.Status = OperationResultStatusType.Failed;
                response.Errors.Add("The department name already exists");
                return response;
            }

            #region Deactivated previous department user records

            if (request.Users != null)
            {
                _userRepository.Remove(request.Users, _httpContextAccessor.HttpContext.GetUserId());
            }

            if (request.DirectorUserId.HasValue)
            {
                _userRepository.Remove(new List<Guid>() { request.DirectorUserId.Value }, _httpContextAccessor.HttpContext.GetUserId());
            }

            #endregion

            response.Body = _repository.Create(_mapper.Map(request, company.Id));
            response.Status = OperationResultStatusType.FullSuccess;

            _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

            return response;
        }
    }
}
