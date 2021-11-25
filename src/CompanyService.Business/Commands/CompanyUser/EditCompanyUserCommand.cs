using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.CompanyService.Business.Commands.CompanyUser.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser;
using LT.DigitalOffice.CompanyService.Validation.CompanyUser.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.CompanyService.Business.Commands.CompanyUser
{
  public class EditCompanyUserCommand : IEditCompanyUserCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly ICompanyUserRepository _repository;
    private readonly IEditCompanyUserRequestValidator _validator;
    private readonly IResponseCreator _responseCreator;
    private readonly ICacheNotebook _cacheNotebook;

    private async Task ClearCache(Guid userId, Guid newPositionId)
    {
      Guid positionId = (await _repository.GetAsync(userId)).PositionId;

      await Task.WhenAll(
        _cacheNotebook.RemoveAsync(positionId),
        _cacheNotebook.RemoveAsync(newPositionId));
    }

    public EditCompanyUserCommand(
      IAccessValidator accessValidator,
      ICompanyUserRepository repository,
      IEditCompanyUserRequestValidator validator,
      IResponseCreator responseCreator,
      ICacheNotebook cacheNotebook)
    {
      _accessValidator = accessValidator;
      _repository = repository;
      _validator = validator;
      _responseCreator = responseCreator;
      _cacheNotebook = cacheNotebook;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(EditCompanyUserRequest request)
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

      bool result = await _repository.EditAsync(request);

      if (result.HasValue)
      {
        await ClearCache(request.UserId request.CompanyId);
      }

      return new OperationResultResponse<bool>
      {
        Status = result ? OperationResultStatusType.FullSuccess : OperationResultStatusType.Failed,
        Body = result
      };
    }
  }
}
