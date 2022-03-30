using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject;
using LT.DigitalOffice.CompanyService.Validation.ContractSubject.Interfaces;
using LT.DigitalOffice.Kernel.Validators;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.CompanyService.Validation.ContractSubject
{
  public class EditContractSubjectRequestValidator : ExtendedEditRequestValidator<Guid, EditContractSubjectRequest>, IEditContractSubjectRequestValidator
  {
    private bool isActive = true;
    private readonly ICompanyUserRepository _companyUserRepository;

    private void HandleInternalPorpetyValidation(
      Operation<EditContractSubjectRequest> requestedOperation,
      CustomContext context)
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

      AddFailureForPropertyIf(
        nameof(EditContractSubjectRequest.Name),
        x => x == OperationType.Replace,
        new()
        {
          { x => !string.IsNullOrEmpty(x.value?.ToString().Trim()), "Name can't be empty." }
        });

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
      ICompanyUserRepository companyUserRepository)
    {
      _companyUserRepository = companyUserRepository;

      RuleForEach(x => x.Item2.Operations)
        .Custom(HandleInternalPorpetyValidation);

      When(x => bool.TryParse(
        x.Item2.Operations.FirstOrDefault(o => o.path.EndsWith(nameof(EditContractSubjectRequest.IsActive), StringComparison.OrdinalIgnoreCase))?.value?.ToString(),
        out isActive)
        && !isActive,
        () =>
        {
          RuleFor(x => x.Item1)
            .MustAsync(async (contractSubjectId, _) =>
            {
              return !await _companyUserRepository.AnyAsync(contractSubjectId);
            });
        });
    }
  }
}
