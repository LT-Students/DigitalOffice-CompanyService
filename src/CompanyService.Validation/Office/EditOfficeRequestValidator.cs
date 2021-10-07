using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using LT.DigitalOffice.CompanyService.Validation.Helper;
using LT.DigitalOffice.CompanyService.Validation.Office.Interfaces;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.CompanyService.Validation.Office
{
  public class EditOfficeRequestValidator : BaseEditRequestValidator<EditOfficeRequest>, IEditOfficeRequestValidator
  {
    private void HandleInternalPropertyValidation(Operation<EditOfficeRequest> requestedOperation, CustomContext context)
    {
      Context = context;
      RequestedOperation = requestedOperation;

      #region Paths

      AddСorrectPaths(
        new()
        {
          nameof(EditOfficeRequest.Name),
          nameof(EditOfficeRequest.City),
          nameof(EditOfficeRequest.Address),
        });

      AddСorrectOperations(nameof(EditOfficeRequest.Name), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditOfficeRequest.City), new() { OperationType.Replace });
      AddСorrectOperations(nameof(EditOfficeRequest.Address), new() { OperationType.Replace });

      #endregion

      #region string property

      AddFailureForPropertyIf(
        nameof(EditOfficeRequest.City),
        x => x == OperationType.Replace,
        new()
        {
          { x => !string.IsNullOrEmpty(x.value?.ToString()), "City cannot be empty." },
        });

      AddFailureForPropertyIf(
        nameof(EditOfficeRequest.Address),
        x => x == OperationType.Replace,
        new()
        {
          { x => !string.IsNullOrEmpty(x.value?.ToString()), "Address cannot be empty." },
        });

      #endregion
    }

    public EditOfficeRequestValidator()
    {
      RuleForEach(x => x.Operations)
        .Custom(HandleInternalPropertyValidation);
    }
  }
}
