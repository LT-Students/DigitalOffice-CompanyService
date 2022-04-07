using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser;
using LT.DigitalOffice.CompanyService.Validation.CompanyUser.Interfaces;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.Models.Broker.Enums;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.CompanyService.Validation.CompanyUser
{
  public class EditCompanyUserRequestValidator : BaseEditRequestValidator<EditCompanyUserRequest>, IEditCompanyUserRequestValidator
  {
    private readonly IContractSubjectRepository _contractSubjectRepository;

    private void HandleInternalPropertyValidation(
      Operation<EditCompanyUserRequest> requestedOperation,
      CustomContext context)
    {
      Context = context;
      RequestedOperation = requestedOperation;

      #region Paths

      AddСorrectPaths(
        new()
        {
          nameof(EditCompanyUserRequest.Rate),
          nameof(EditCompanyUserRequest.StartWorkingAt)
        });

      AddСorrectOperations(nameof(EditCompanyUserRequest.Rate), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditCompanyUserRequest.StartWorkingAt), new() { OperationType.Replace });

      #endregion

      #region ContractSubjectId

      AddFailureForPropertyIfAsync(
        nameof(EditCompanyUserRequest.ContractSubjectId),
        x => x == OperationType.Replace,
        new()
        {
          {
            async (x) =>
            {
              if (x.value?.ToString() is null)
              {
                return true;
              }

              if (!Guid.TryParse(x.value.ToString(), out Guid id))
              {
                return false;
              }

              return !await _contractSubjectRepository.DoesExistAsync(id);
            },
            "Wrong contract subject id value."
          }
        });

      #endregion

      #region StartWorkingAt

      AddFailureForPropertyIf(
        nameof(EditCompanyUserRequest.StartWorkingAt),
        x => x == OperationType.Replace,
        new Dictionary<Func<Operation<EditCompanyUserRequest>, bool>, string>
        {
          {
            x => x.value?.ToString() is null ? false :
              DateTime.TryParse(x.value.ToString(), out DateTime result),
            "Start working at has incorrect format."
          },
        });

      #endregion

      #region ContractTermType

      AddFailureForPropertyIf(
        nameof(EditCompanyUserRequest.ContractTermType),
        x => x == OperationType.Replace,
        new()
        {
          {
            x => Enum.IsDefined(typeof(ContractTerm), x.value?.ToString()),
            "Wrong contract term type."
          },
        });

      #endregion

      #region Rate

      AddFailureForPropertyIf(
        nameof(EditCompanyUserRequest.Rate),
        x => x == OperationType.Replace,
        new()
        {
          { x => x.value?.ToString() is null ? true : 
              double.TryParse(x.value.ToString(), out double rate) && rate > 0 && rate <= 1,
            "The rate must be between 0 and 1." },
        });

      #endregion

      #region EndWorkingAt

      AddFailureForPropertyIf(
        nameof(EditCompanyUserRequest.EndWorkingAt),
        x => x == OperationType.Replace,
        new Dictionary<Func<Operation<EditCompanyUserRequest>, bool>, string>
        {
          {
            x => x.value?.ToString() is null ? true :
              DateTime.TryParse(x.value.ToString(), out DateTime result),
            "End working data has incorrect format."
          },
        });

      #endregion

      #region Probation

      AddFailureForPropertyIf(
        nameof(EditCompanyUserRequest.Probation),
        x => x == OperationType.Replace,
        new Dictionary<Func<Operation<EditCompanyUserRequest>, bool>, string>
        {
          {
            x => x.value?.ToString() is null ? true :
              DateTime.TryParse(x.value.ToString(), out DateTime result),
            "Probation date has incorrect format."
          },
        });

      #endregion
    }

    public EditCompanyUserRequestValidator(
      IContractSubjectRepository contractSubjectRepository)
    {
      _contractSubjectRepository = contractSubjectRepository;

      RuleForEach(x => x.Operations)
        .Custom(HandleInternalPropertyValidation);
    }
  }
}
