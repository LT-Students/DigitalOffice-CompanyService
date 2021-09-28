using System.Collections.Generic;
using System.Linq;
using System.Net;
using LT.DigitalOffice.CompanyService.Business.Commands.Office.Interface;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Office
{
  public class FindOfficesCommand : IFindOfficesCommand
  {
    private readonly IOfficeRepository _officeRepository;
    private readonly IOfficeInfoMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IBaseFindRequestValidator _baseFindValidator;


    public FindOfficesCommand(
      IOfficeRepository officeRepository,
      IOfficeInfoMapper mapper,
      IHttpContextAccessor httpContextAccessor,
      IBaseFindRequestValidator baseFindValidator)
    {
      _officeRepository = officeRepository;
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
      _baseFindValidator = baseFindValidator;
    }

    public FindResultResponse<OfficeInfo> Execute(BaseFindFilter filter)
    {
      FindResultResponse<OfficeInfo> response = new();

      if (!_baseFindValidator.ValidateCustom(filter, out List<string> errors))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        response.Status = OperationResultStatusType.Failed;
        response.Errors = errors;
        return response;
      }

      response.Body = _officeRepository
        .Find(filter, out int totalCount)
        .Select(_mapper.Map)
        .ToList();

      response.TotalCount = totalCount;
      response.Status = OperationResultStatusType.FullSuccess;

      if (response.Body == null)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;

        response.Errors = new() { "Office was not found." };
        response.Status = OperationResultStatusType.Failed;
      }

      return response;
    }
  }
}
