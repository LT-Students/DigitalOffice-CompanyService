using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Business.Helper;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Company.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company
{
  public class CreateCompanyCommand : ICreateCompanyCommand
  {
    private readonly IDbCompanyMapper _mapper;
    private readonly IAccessValidator _accessValidator;
    private readonly ICreateCompanyRequestValidator _validator;
    private readonly ICompanyRepository _repository;
    private readonly ICompanyChangesRepository _companyChangesRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreator _responseCreator;

    public CreateCompanyCommand(
      IDbCompanyMapper mapper,
      IAccessValidator accessValidator,
      ICreateCompanyRequestValidator validator,
      ICompanyRepository repository,
      ICompanyChangesRepository companyChangesRepository,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreator responseCreator)
    {
      _mapper = mapper;
      _accessValidator = accessValidator;
      _validator = validator;
      _repository = repository;
      _companyChangesRepository = companyChangesRepository;
      _httpContextAccessor = httpContextAccessor;
      _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateCompanyRequest request)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveCompanies))
      {
        return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.Forbidden);
      }

      if (!_validator.ValidateCustom(request, out List<string> errors))
      {
        return _responseCreator.CreateFailureResponse<Guid?>(
          HttpStatusCode.BadRequest,
          errors);
      }

      DbCompany company = await _mapper.MapAsync(request);

      await _repository.CreateAsync(company);

      //TODO async
      //Task.Run(() =>
      //{
      await _companyChangesRepository.CreateAsync(
        company.Id,
        null,
        CreateHistoryMessageHelper.Create(company));
      //}

      _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

      return new OperationResultResponse<Guid?>
      {
        Status = OperationResultStatusType.FullSuccess,
        Body = company.Id
      };
    }
  }
}
