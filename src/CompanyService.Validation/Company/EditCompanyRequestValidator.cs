using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company;
using LT.DigitalOffice.CompanyService.Validation.Company.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Validators;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;

namespace LT.DigitalOffice.CompanyService.Validation.Company
{
  public class EditCompanyRequestValidator : BaseEditRequestValidator<EditCompanyRequest>, IEditCompanyRequestValidator
  {
    private void HandleInternalPropertyValidation(Operation<EditCompanyRequest> requestedOperation, CustomContext context)
    {
      Context = context;
      RequestedOperation = requestedOperation;

      #region Paths

      AddСorrectPaths(
        new List<string>
        {
          nameof(EditCompanyRequest.Name),
          nameof(EditCompanyRequest.Description),
          nameof(EditCompanyRequest.Tagline),
          nameof(EditCompanyRequest.Contacts),
          nameof(EditCompanyRequest.Logo)
        });

      AddСorrectOperations(nameof(EditCompanyRequest.Name), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditCompanyRequest.Description), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditCompanyRequest.Tagline), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditCompanyRequest.Contacts), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditCompanyRequest.Logo), new List<OperationType> { OperationType.Replace });

      #endregion

      #region CompanyName

      AddFailureForPropertyIf(
          nameof(EditCompanyRequest.Name),
          x => x == OperationType.Replace,
          new()
          {
            { x => !string.IsNullOrEmpty(x.value.ToString()), "CompanyName is too short" },
          });

      #endregion

      #region Logo

      AddFailureForPropertyIf(
        nameof(EditCompanyRequest.Logo),
        x => x == OperationType.Replace,
        new()
        {
          {
            x =>
            {
              try
              {
                AddImageRequest image = JsonConvert.DeserializeObject<AddImageRequest>(x.value?.ToString());

                Span<byte> byteString = new Span<byte>(new byte[image.Content.Length]);

                if (!String.IsNullOrEmpty(image.Content) &&
                  Convert.TryFromBase64String(image.Content, byteString, out _) &&
                  ImageFormats.formats.Contains(image.Extension))
                {
                  return true;
                }
              }
              catch
              {
              }
              return false;
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
