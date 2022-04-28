using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Business.Helper;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company;
using LT.DigitalOffice.CompanyService.Validation.Company.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company
{
  public class EditCompanyCommand : IEditCompanyCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IPatchDbCompanyMapper _mapper;
    private readonly ICompanyRepository _companyRepository;
    private readonly IEditCompanyRequestValidator _validator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreator _responseCreator;
    private readonly ICompanyChangesRepository _companyChangesRepository;
    private readonly IGlobalCacheRepository _globalCache;

    
    public EditCompanyCommand(
      IAccessValidator accessValidator,
      IPatchDbCompanyMapper mapper,
      ICompanyRepository companyRepository,
      IEditCompanyRequestValidator validator,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreator responseCreator,
      ICompanyChangesRepository companyChangesRepository,
      IGlobalCacheRepository globalCache)
    {
      _accessValidator = accessValidator;
      _mapper = mapper;
      _companyRepository = companyRepository;
      _validator = validator;
      _httpContextAccessor = httpContextAccessor;
      _responseCreator = responseCreator;
      _companyChangesRepository = companyChangesRepository;
      _globalCache = globalCache;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid companyId, JsonPatchDocument<EditCompanyRequest> request)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveCompanyData)
        && !await _accessValidator.HasRightsAsync(Rights.AddEditRemoveCompanies))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      ValidationResult validationResult = await _validator.ValidateAsync((companyId, request));
      if (validationResult.IsValid)
      {
        return _responseCreator.CreateFailureResponse<bool>(
          HttpStatusCode.BadRequest,
          validationResult.Errors.Select(x => x.ErrorMessage).ToList());
      }

      Operation<EditCompanyRequest> imageOperation = request.Operations.FirstOrDefault(o => o.path.EndsWith(nameof(EditCompanyRequest.Logo), StringComparison.OrdinalIgnoreCase));

      JsonPatchDocument<DbCompany> dbRequest = await _mapper.MapAsync(request);

      await _companyRepository.EditAsync(companyId, dbRequest);

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

      await _globalCache.RemoveAsync(companyId);

      return new OperationResultResponse<bool>
      {
        Body = true
      };
    }
  }
}
