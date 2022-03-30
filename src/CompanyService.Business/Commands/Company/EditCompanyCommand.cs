using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Business.Helper;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company;
using LT.DigitalOffice.CompanyService.Validation.Company.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Requests.Email;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company
{
  public class EditCompanyCommand : IEditCompanyCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly ILogger<EditCompanyCommand> _logger;
    private readonly IPatchDbCompanyMapper _mapper;
    private readonly ICompanyRepository _companyRepository;
    private readonly IEditCompanyRequestValidator _validator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRequestClient<ICreateSmtpCredentialsRequest> _rcCreateSmtp;
    private readonly ICompanyChangesRepository _companyChangesRepository;
    private readonly IGlobalCacheRepository _globalCache;

    
    public EditCompanyCommand(
      IAccessValidator accessValidator,
      ILogger<EditCompanyCommand> logger,
      IPatchDbCompanyMapper mapper,
      ICompanyRepository companyRepository,
      IEditCompanyRequestValidator validator,
      IRequestClient<ICreateSmtpCredentialsRequest> rcCreateSmtp,
      ICompanyChangesRepository companyChangesRepository,
      IHttpContextAccessor httpContextAccessor,
      IGlobalCacheRepository globalCache)
    {
      _accessValidator = accessValidator;
      _logger = logger;
      _mapper = mapper;
      _companyRepository = companyRepository;
      _validator = validator;
      _rcCreateSmtp = rcCreateSmtp;
      _companyChangesRepository = companyChangesRepository;
      _httpContextAccessor = httpContextAccessor;
      _globalCache = globalCache;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(JsonPatchDocument<EditCompanyRequest> request)
    {
      if (!await _accessValidator.IsAdminAsync())
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        return new OperationResultResponse<bool>
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { "Not enouth rights." }
        };
      }

      if (await _companyRepository.GetAsync() == null)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;

        return new OperationResultResponse<bool>
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { "Company does not exist." }
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

      Operation<EditCompanyRequest> imageOperation = request.Operations.FirstOrDefault(o => o.path.EndsWith(nameof(EditCompanyRequest.Logo), StringComparison.OrdinalIgnoreCase));

      JsonPatchDocument<DbCompany> dbRequest = _mapper.Map(request);

      await _companyRepository.EditAsync(dbRequest);

      DbCompany company = null;

      //TODO async
      //Task.Run(() =>
      //{
      company ??= await _companyRepository.GetAsync();
      await _companyChangesRepository.CreateAsync(
        company.Id,
        _httpContextAccessor.HttpContext.GetUserId(),
        CreateHistoryMessageHelper.Create(company, dbRequest));
      //});

      await _globalCache.RemoveAsync(company.Id);

      return new OperationResultResponse<bool>
      {
        Status = errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess,
        Body = true,
        Errors = errors
      };
    }
  }
}
