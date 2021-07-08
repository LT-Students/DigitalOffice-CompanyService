using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Position;
using LT.DigitalOffice.CompanyService.Validation.Helper;
using LT.DigitalOffice.CompanyService.Validation.Position.Interfaces;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Validation.Position
{
    public class EditPositionRequestValidator : BaseEditRequestValidator<EditPositionRequest>, IEditPositionRequestValidator
    {
        private void HandleInternalPropertyValidation(Operation<EditPositionRequest> requestedOperation, CustomContext context)
        {
            RequestedOperation = requestedOperation;
            Context = context;

            #region Paths

            AddСorrectPaths(
                new List<string>
                {
                    nameof(EditPositionRequest.Name),
                    nameof(EditPositionRequest.Description),
                    nameof(EditPositionRequest.IsActive)
                });

            AddСorrectOperations(nameof(EditPositionRequest.Name), new() { OperationType.Replace });
            AddСorrectOperations(nameof(EditPositionRequest.Description), new() { OperationType.Replace });
            AddСorrectOperations(nameof(EditPositionRequest.IsActive), new() { OperationType.Replace });

            #endregion

            #region Name

            AddFailureForPropertyIf(
                nameof(EditPositionRequest.Name),
                x => x == OperationType.Replace,
                new()
                {
                    { x => !string.IsNullOrEmpty(x.value.ToString()), "Name is too short." },
                });

            #endregion

            #region Description

            AddFailureForPropertyIf(
                nameof(EditPositionRequest.Description),
                x => x == OperationType.Replace,
                new()
                {
                    { x => !string.IsNullOrEmpty(x.value.ToString()), "Description is too short." },
                });

            #endregion

            #region IsActive

            AddFailureForPropertyIf(
                nameof(EditPositionRequest.IsActive),
                x => x == OperationType.Replace,
                new()
                {
                    { x => bool.TryParse(x.value.ToString(), out bool _), "Incorrect format of IsActive." },
                });

            #endregion
        }

        public EditPositionRequestValidator()
        {
            RuleForEach(x => x.Operations)
               .Custom(HandleInternalPropertyValidation);
        }
    }
}