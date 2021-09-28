using System;
using System.Collections.Generic;
using System.Net;
using LT.DigitalOffice.CompanyService.Business.Commands.Office.Interface;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using LT.DigitalOffice.CompanyService.Validation.Office.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Office
{
  public class EditOfficeCommand : IEditOfficeCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IOfficeRepository _officeRepository;
    private readonly IPatchDbOfficeMapper _mapper;
    private readonly IEditOfficeRequestValidator _validator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EditOfficeCommand(
      IAccessValidator accessValidator,
      IOfficeRepository officeRepository,
      IPatchDbOfficeMapper mapper,
      IEditOfficeRequestValidator validator,
      IHttpContextAccessor httpContextAccessor)
    {
      _accessValidator = accessValidator;
      _officeRepository = officeRepository;
      _mapper = mapper;
      _validator = validator;
      _httpContextAccessor = httpContextAccessor;
    }

    public OperationResultResponse<bool> Execute(Guid officeId, JsonPatchDocument<EditOfficeRequest> request)
    {
      if (!_accessValidator.HasRights(Rights.AddEditRemoveDepartments))
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

      OperationResultResponse<bool> response = new();

      response.Body = _officeRepository.Edit(officeId, _mapper.Map(request));
      response.Status = OperationResultStatusType.FullSuccess;

      if (!response.Body)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        response.Status = OperationResultStatusType.Failed;
      }

      return response;
    }
  }
}
