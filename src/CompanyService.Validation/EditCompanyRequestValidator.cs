using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Validation
{
    public class EditCompanyRequestValidator : AbstractValidator<JsonPatchDocument<EditCompanyRequest>>, IEditCompanyRequestValidator
    {
        private void HandleInternalPropertyValidation(Operation<EditCompanyRequest> requestedOperation, CustomContext context)
        {
            #region local functions

            void AddСorrectPaths(List<string> paths)
            {
                if (paths.FirstOrDefault(p => p.EndsWith(requestedOperation.path[1..], StringComparison.OrdinalIgnoreCase)) == null)
                {
                    context.AddFailure(requestedOperation.path, $"This path {requestedOperation.path} is not available");
                }
            }

            void AddСorrectOperations(
                string propertyName,
                List<OperationType> types)
            {
                if (requestedOperation.path.EndsWith(propertyName, StringComparison.OrdinalIgnoreCase)
                    && !types.Contains(requestedOperation.OperationType))
                {
                    context.AddFailure(propertyName, $"This operation {requestedOperation.OperationType} is prohibited for {propertyName}");
                }
            }

            void AddFailureForPropertyIf(
                string propertyName,
                Func<OperationType, bool> type,
                Dictionary<Func<Operation<EditCompanyRequest>, bool>, string> predicates)
            {
                if (!requestedOperation.path.EndsWith(propertyName, StringComparison.OrdinalIgnoreCase)
                    || !type(requestedOperation.OperationType))
                {
                    return;
                }

                foreach (var validateDelegate in predicates)
                {
                    if (!validateDelegate.Key(requestedOperation))
                    {
                        context.AddFailure(propertyName, validateDelegate.Value);
                    }
                }
            }

            #endregion

            #region paths

            AddСorrectPaths(
                new List<string>
                {
                    nameof(EditCompanyRequest.PortalName),
                    nameof(EditCompanyRequest.CompanyName),
                    nameof(EditCompanyRequest.Description),
                    nameof(EditCompanyRequest.SiteUrl),
                    nameof(EditCompanyRequest.Host),
                    nameof(EditCompanyRequest.Port),
                    nameof(EditCompanyRequest.Email),
                    nameof(EditCompanyRequest.Password),
                    nameof(EditCompanyRequest.EnableSsl),
                    nameof(EditCompanyRequest.Logo)
                });

            AddСorrectOperations(nameof(EditCompanyRequest.PortalName), new List<OperationType> { OperationType.Replace });
            AddСorrectOperations(nameof(EditCompanyRequest.CompanyName), new List<OperationType> { OperationType.Replace });
            AddСorrectOperations(nameof(EditCompanyRequest.Description), new List<OperationType> { OperationType.Replace });
            AddСorrectOperations(nameof(EditCompanyRequest.SiteUrl), new List<OperationType> { OperationType.Replace });
            AddСorrectOperations(nameof(EditCompanyRequest.Host), new List<OperationType> { OperationType.Replace });
            AddСorrectOperations(nameof(EditCompanyRequest.Port), new List<OperationType> { OperationType.Replace });
            AddСorrectOperations(nameof(EditCompanyRequest.Email), new List<OperationType> { OperationType.Replace });
            AddСorrectOperations(nameof(EditCompanyRequest.Password), new List<OperationType> { OperationType.Replace });
            AddСorrectOperations(nameof(EditCompanyRequest.EnableSsl), new List<OperationType> { OperationType.Replace });
            AddСorrectOperations(nameof(EditCompanyRequest.Logo), new List<OperationType> { OperationType.Replace, OperationType.Add, OperationType.Remove });

            #endregion

            #region PortalName

            AddFailureForPropertyIf(
                nameof(EditCompanyRequest.PortalName),
                x => x == OperationType.Replace,
                new()
                {
                    { x => !string.IsNullOrEmpty(x.value.ToString()), "PortalName is too short" },
                });

            #endregion

            #region CompanyName

            AddFailureForPropertyIf(
                nameof(EditCompanyRequest.CompanyName),
                x => x == OperationType.Replace,
                new()
                {
                    { x => !string.IsNullOrEmpty(x.value.ToString()), "CompanyName is too short" },
                });

            #endregion

            #region AvatarImage

            AddFailureForPropertyIf(
                nameof(EditCompanyRequest.Logo),
                x => x == OperationType.Replace,
                new()
                {
                    { x =>
                        {
                            try
                            {
                                _ = JsonConvert.DeserializeObject<AddImageRequest>(x.value?.ToString());
                                return true;
                            }
                            catch
                            {
                                return false;
                            }
                        },
                        "Incorrect Image format"
                    }
            });

            #endregion
        }

        public EditCompanyRequestValidator()
        {
            RuleForEach(x => x.Operations)
               .Custom(HandleInternalPropertyValidation);
        }
    }
}
