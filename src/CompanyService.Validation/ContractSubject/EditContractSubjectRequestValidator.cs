using System;
using System.Threading.Tasks;
using FluentValidation;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject;
using LT.DigitalOffice.CompanyService.Validation.ContractSubject.Interfaces;
using LT.DigitalOffice.Kernel.Validators;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.CompanyService.Validation.ContractSubject
{
  public class EditContractSubjectRequestValidator : ExtendedEditRequestValidator<Guid, EditContractSubjectRequest>, IEditContractSubjectRequestValidator
  {
    private readonly IContractSubjectRepository _contractSubjectRepository;

    private async Task HandleInternalPorpetyValidationAsync(
      Operation<EditContractSubjectRequest> requestedOperation,
      ValidationContext<(Guid, JsonPatchDocument<EditContractSubjectRequest>)> context)
    {
      Context = context;
      RequestedOperation = requestedOperation;

      #region Paths

      AddСorrectPaths(
        new()
        {
          nameof(EditContractSubjectRequest.Name),
          nameof(EditContractSubjectRequest.Description),
          nameof(EditContractSubjectRequest.IsActive)
        });

      AddСorrectOperations(nameof(EditContractSubjectRequest.Name), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditContractSubjectRequest.Description), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditContractSubjectRequest.IsActive), new() { OperationType.Replace });

      #endregion

      #region Name

      await AddFailureForPropertyIfAsync(
        nameof(EditContractSubjectRequest.Name),
        x => x == OperationType.Replace,
        new()
        {
          { x => Task.FromResult(!string.IsNullOrEmpty(x.value?.ToString().Trim())), "Name can't be empty." },
          { x => Task.FromResult(x.value?.ToString().Length < 151), "Contract subject's name is too long." },
          { async x => !await _contractSubjectRepository.DoesNameExistAsync(x.value?.ToString()), "Name already exists."}
        },
        CascadeMode.Stop);

      #endregion

      #region IsActive

      AddFailureForPropertyIf(
        nameof(EditContractSubjectRequest.IsActive),
        x => x == OperationType.Replace,
        new()
        {
          { x => bool.TryParse(x.value?.ToString(), out _), "Incorrect IsActive value." }
        });

      #endregion
    }

    public EditContractSubjectRequestValidator(
      IContractSubjectRepository contractSubjectRepository)
    {
      _contractSubjectRepository = contractSubjectRepository;

      RuleForEach(x => x.Item2.Operations)
        .CustomAsync(async (x, context, _) => await HandleInternalPorpetyValidationAsync(x, context));
    }
  }
}
