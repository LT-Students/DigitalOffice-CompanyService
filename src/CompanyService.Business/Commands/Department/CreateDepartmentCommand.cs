using LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.CompanyService.Validation.Department.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using System;

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

        public CreateDepartmentCommand(
            IDepartmentRepository repository,
            ICompanyRepository companyRepository,
            IDepartmentUserRepository userRepository,
            ICreateDepartmentRequestValidator validator,
            IDbDepartmentMapper mapper,
            IAccessValidator accessValidator)
        {
            _repository = repository;
            _companyRepository = companyRepository;
            _userRepository = userRepository;
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
                response.Status = OperationResultStatusType.Conflict;
                response.Errors.Add("The department name already exists");
                return response;
            }

            #region Deactivated previous department user records

            if (request.Users != null) 
            {
                foreach (Guid userId in request.Users)
                {
                    _userRepository.Remove(userId);
                }
            }

            if (request.DirectorUserId.HasValue) 
            {
                _userRepository.Remove(request.DirectorUserId.Value);
            }

            #endregion

            response.Body = _repository.CreateDepartment(_mapper.Map(request, company.Id));
            response.Status = OperationResultStatusType.FullSuccess;
            return response;
        }
    }
}
