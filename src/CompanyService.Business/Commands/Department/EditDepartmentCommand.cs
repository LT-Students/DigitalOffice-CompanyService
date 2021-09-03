﻿using System;
using System.Collections.Generic;
using System.Net;
using LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.CompanyService.Validation.Department.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Department
{
  public class EditDepartmentCommand : IEditDepartmentCommand
  {
    private readonly IEditDepartmentRequestValidator _validator;
    private readonly IDepartmentRepository _repository;
    private readonly IPatchDbDepartmentMapper _mapper;
    private readonly IAccessValidator _accessValidator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EditDepartmentCommand(
        IEditDepartmentRequestValidator validator,
        IDepartmentRepository repository,
        IPatchDbDepartmentMapper mapper,
        IAccessValidator accessValidator,
        IHttpContextAccessor httpContextAccessor)
    {
      _validator = validator;
      _repository = repository;
      _mapper = mapper;
      _accessValidator = accessValidator;
      _httpContextAccessor = httpContextAccessor;
    }

    public OperationResultResponse<bool> Execute(Guid departmentId, JsonPatchDocument<EditDepartmentRequest> request)
    {
      if (!(_accessValidator.IsAdmin() ||
            _accessValidator.HasRights(Rights.AddEditRemoveDepartments)))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        return new OperationResultResponse<bool>
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { "Not enough rights." }
        };
      }

      if (!_validator.ValidateCustom(request, out List<string> errors))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return new OperationResultResponse<bool>
        {
          Status = OperationResultStatusType.Failed,
          Errors = errors
        };
      }

      DbDepartment dbDepartment = _repository.Get(new GetDepartmentFilter { DepartmentId = departmentId });

      if (dbDepartment == null)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;

        return new OperationResultResponse<bool>
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { $"Department with id: '{departmentId}' doesn't exist." }
        };
      }

      OperationResultResponse<bool> response = new();

      foreach (var item in request.Operations)
      {
        if (item.path.EndsWith(nameof(EditDepartmentRequest.Name), StringComparison.OrdinalIgnoreCase) &&
            _repository.DoesNameExist(item.value.ToString()))
        {
          _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;

          response.Status = OperationResultStatusType.Failed;
          response.Errors.Add("The department name already exists");
          return response;
        }
      }

      response.Body = _repository.Edit(dbDepartment, _mapper.Map(request));
      response.Status = response.Body ? OperationResultStatusType.FullSuccess : OperationResultStatusType.Failed;
      return response;
    }
  }
}
