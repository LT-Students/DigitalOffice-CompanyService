using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.CompanyService.Business.Commands.CompanyUser.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser;
using LT.DigitalOffice.CompanyService.Validation.CompanyUser.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.CompanyService.Business.Commands.CompanyUser
{
  public class CreateCompanyUserCommand : ICreateCompanyUserCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly ICreateCompanyUserRequestValidator _validator;
    private readonly IDbCompanyUserMapper _mapper;
    private readonly ICompanyUserRepository _repository;
    private readonly IResponseCreator _responseCreator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateCompanyUserCommand(
      IAccessValidator accessValidator,
      ICreateCompanyUserRequestValidator validator,
      IDbCompanyUserMapper mapper,
      ICompanyUserRepository repository,
      IResponseCreator responseCreator,
      IHttpContextAccessor httpContextAccessor)
    {
      _accessValidator = accessValidator;
      _validator = validator;
      _mapper = mapper;
      _repository = repository;
      _responseCreator = responseCreator;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateCompanyUserRequest request)
    {
      if (!await _accessValidator.HasAnyRightAsync(Rights.AddEditRemoveCompanyData, Rights.AddEditRemoveCompanies))
      {
        return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.Forbidden);
      }

      ValidationResult validationResult = await _validator.ValidateAsync(request);

      if (!validationResult.IsValid)
      {
        return _responseCreator.CreateFailureResponse<Guid?>(
          HttpStatusCode.BadRequest,
          validationResult.Errors.Select(vf => vf.ErrorMessage).ToList());
      }

      DbCompanyUser dbCompanyUser = _mapper.Map(request);
      await _repository.CreateAsync(dbCompanyUser);
      _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

      return new OperationResultResponse<Guid?>(body: dbCompanyUser.Id);
    }
  }
}
