using LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.CompanyService.Validation.Department.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;
using System;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Department
{
    public class EditDepartmentCommand : IEditDepartmentCommand
    {
        private readonly IEditDepartmentRequestValidator _validator;
        private readonly IDepartmentRepository _repository;
        private readonly IPatchDbDepartmentMapper _mapper;
        private readonly IAccessValidator _accessValidator;

        public EditDepartmentCommand(
            IEditDepartmentRequestValidator validator,
            IDepartmentRepository repository,
            IPatchDbDepartmentMapper mapper,
            IAccessValidator accessValidator)
        {
            _validator = validator;
            _repository = repository;
            _mapper = mapper;
            _accessValidator = accessValidator;
        }

        public OperationResultResponse<bool> Execute(Guid departmentId, JsonPatchDocument<EditDepartmentRequest> request)
        {
            if (!(_accessValidator.IsAdmin() ||
                  _accessValidator.HasRights(Rights.AddEditRemoveDepartments)))
            {
                throw new ForbiddenException("Not enough rights.");
            }

            _validator.ValidateAndThrowCustom(request);

            OperationResultResponse<bool> response = new();

            foreach (var item in request.Operations)
            {
                if (item.path.EndsWith(nameof(EditDepartmentRequest.Name), StringComparison.OrdinalIgnoreCase) &&
                    _repository.IsNameExist(item.value.ToString()))
                {
                    response.Status = OperationResultStatusType.Conflict;
                    response.Errors.Add("The department name already exists");
                    return response;
                }
            }

            response.Body = _repository.Edit(departmentId, _mapper.Map(request));
            response.Status = response.Body ? OperationResultStatusType.FullSuccess : OperationResultStatusType.Failed;
            return response;
        }
    }
}
