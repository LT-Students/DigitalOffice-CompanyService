using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.CompanyService.Business.Commands.CompanyUser.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser;
using LT.DigitalOffice.CompanyService.Validation.CompanyUser.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Business.Commands.CompanyUser
{
  public class EditCompanyUserCommand : IEditCompanyUserCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly ICompanyUserRepository _repository;
    private readonly IEditCompanyUserRequestValidator _validator;
    private readonly IResponseCreator _responseCreator;
    private readonly IPatchCompanyUserMapper _mapper;
    private readonly IGlobalCacheRepository _globalCache;

    private async Task ClearCache(Guid userId)
    {
      Guid companyId = (await _repository.GetAsync(userId)).CompanyId;

      await _globalCache.RemoveAsync(companyId);
    }

    public EditCompanyUserCommand(
      IAccessValidator accessValidator,
      ICompanyUserRepository repository,
      IEditCompanyUserRequestValidator validator,
      IResponseCreator responseCreator,
      IGlobalCacheRepository globalCache,
      IPatchCompanyUserMapper mapper)
    {
      _accessValidator = accessValidator;
      _repository = repository;
      _validator = validator;
      _responseCreator = responseCreator;
      _globalCache = globalCache;
      _mapper = mapper;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(
      Guid userId,
      JsonPatchDocument<EditCompanyUserRequest> request)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveUsers))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      ValidationResult validationResult = await _validator.ValidateAsync(request);
      if (!validationResult.IsValid)
      {
        return _responseCreator.CreateFailureResponse<bool>(
          HttpStatusCode.BadRequest,
          validationResult.Errors.Select(e => e.ErrorMessage).ToList());
      }

      bool result = await _repository.EditAsync(userId, _mapper.Map(request));

      if (result)
      {
        await ClearCache(userId);
      }

      return new OperationResultResponse<bool>
      {
        Status = result ? OperationResultStatusType.FullSuccess : OperationResultStatusType.Failed,
        Body = result
      };
    }
  }
}
