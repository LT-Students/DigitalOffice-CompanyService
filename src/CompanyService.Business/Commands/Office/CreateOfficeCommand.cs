using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Business.Commands.Office.Interface;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using LT.DigitalOffice.CompanyService.Validation.Office.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Office
{
  public class CreateOfficeCommand : ICreateOfficeCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IOfficeRepository _officeRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IDbOfficeMapper _mapper;
    private readonly ICreateOfficeRequestValidator _validator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateOfficeCommand(
      IAccessValidator accessValidator,
      IOfficeRepository officeRepository,
      ICompanyRepository companyRepository,
      IDbOfficeMapper mapper,
      ICreateOfficeRequestValidator validator,
      IHttpContextAccessor httpContextAccessor)
    {
      _accessValidator = accessValidator;
      _officeRepository = officeRepository;
      _companyRepository = companyRepository;
      _mapper = mapper;
      _validator = validator;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<OperationResultResponse<Guid>> ExecuteAsync(CreateOfficeRequest request)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.EditCompany))
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

      DbOffice office = _mapper.Map(request, (await _companyRepository.GetAsync()).Id);
      await _officeRepository.CreateAsync(office);

      _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

      return new OperationResultResponse<Guid>
      {
        Status = OperationResultStatusType.FullSuccess,
        Body = office.Id
      };
    }
  }
}
