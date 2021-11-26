using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser;
using LT.DigitalOffice.CompanyService.Validation.CompanyUser.Interfaces;
using LT.DigitalOffice.Kernel.Validators;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.CompanyService.Validation.CompanyUser
{
  public class EditCompanyUserRequestValidator : BaseEditRequestValidator<EditCompanyUserRequest>, IEditCompanyUserRequestValidator
  {
    private async Task HandleInternalPropertyValidation(
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

      #region Validation

      AddFailureForPropertyIf(
        nameof(EditCompanyUserRequest.StartWorkingAt),
        x => x == OperationType.Replace,
        new Dictionary<Func<Operation<EditCompanyUserRequest>, bool>, string>
        {
          {
            x => string.IsNullOrEmpty(x.value?.ToString())? true :
              DateTime.TryParse(x.value.ToString(), out DateTime result),
            "Start working at has incorrect format."
          },
        });

      AddFailureForPropertyIf(
        nameof(EditCompanyUserRequest.Rate),
        x => x == OperationType.Replace,
        new()
        {
          { x => (x.value?.ToString().Length >= 0 && x.value?.ToString().Length <= 1),
              "The rate must be between 0 and 1." },
        });

      #endregion
    }
    public EditCompanyUserRequestValidator()
    {
      RuleForEach(x => x.Operations)
            .CustomAsync(async (x, context, token) => await HandleInternalPropertyValidation(x, context));
    }
  }
}
