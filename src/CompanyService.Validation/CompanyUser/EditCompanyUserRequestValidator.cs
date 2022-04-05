using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser;
using LT.DigitalOffice.CompanyService.Validation.CompanyUser.Interfaces;
using LT.DigitalOffice.Kernel.Validators;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.CompanyService.Validation.CompanyUser
{
  public class EditCompanyUserRequestValidator : BaseEditRequestValidator<EditCompanyUserRequest>, IEditCompanyUserRequestValidator
  {
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

      #region StartWorkingAt

      AddFailureForPropertyIf(
        nameof(EditCompanyUserRequest.StartWorkingAt),
        x => x == OperationType.Replace,
        new Dictionary<Func<Operation<EditCompanyUserRequest>, bool>, string>
        {
          {
            x => x.value?.ToString() is null ? true :
              DateTime.TryParse(x.value.ToString(), out DateTime result),
            "Start working at has incorrect format."
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
    }

    public EditCompanyUserRequestValidator()
    {
      RuleForEach(x => x.Operations)
        .Custom(HandleInternalPropertyValidation);
    }
  }
}
