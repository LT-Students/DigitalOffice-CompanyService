using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.CompanyService.Business.Commands.ContractSubject.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject;
using LT.DigitalOffice.CompanyService.Validation.ContractSubject.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Business.Commands.ContractSubject
{
  public class EditContractSubjectCommand : IEditContractSubjectCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IContractSubjectRepository _contractSubjectRepository;
    private readonly ICompanyUserRepository _companyUserRepository;
    private readonly IEditContractSubjectRequestValidator _editValidator;
    private readonly IResponseCreator _responseCreator;
    private readonly IPatchContractSubjectMapper _contractSubjectMapper;

    public EditContractSubjectCommand(
      IAccessValidator accessValidator,
      IContractSubjectRepository contractSubjectRepository,
      ICompanyUserRepository companyUserRepository,
      IEditContractSubjectRequestValidator editValidator,
      IResponseCreator responseCreator,
      IPatchContractSubjectMapper contractSubjectMapper)
    {
      _accessValidator = accessValidator;
      _contractSubjectMapper = contractSubjectMapper;
      _editValidator = editValidator;
      _contractSubjectRepository = contractSubjectRepository;
      _companyUserRepository = companyUserRepository;
      _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid contractSubjectId, JsonPatchDocument<EditContractSubjectRequest> request)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveCompanyData)
        || !await _accessValidator.HasRightsAsync(Rights.AddEditRemoveCompanies))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      ValidationResult validationResults = await _editValidator.ValidateAsync((contractSubjectId, request));
      if (!validationResults.IsValid)
      {
        return _responseCreator.CreateFailureResponse<bool>(
          HttpStatusCode.Forbidden,
          validationResults.Errors.Select(vf => vf.ErrorMessage).ToList());
      }

      OperationResultResponse<bool> result = new();

      result.Body = await _contractSubjectRepository.EditAsync(contractSubjectId, _contractSubjectMapper.Map(request));

      bool isActive = true;

      if (result.Body
        && bool.TryParse(request.Operations.FirstOrDefault(
          o => o.path.EndsWith(nameof(EditContractSubjectRequest.IsActive), StringComparison.OrdinalIgnoreCase))?.value?.ToString(),
          out isActive)
        && !isActive)
      {
        await _companyUserRepository.RemoveContractSubjectAsync(contractSubjectId);
      }

      result.Status = result.Body
        ? OperationResultStatusType.FullSuccess
        : OperationResultStatusType.Failed;

      return result;
    }
  }
}
