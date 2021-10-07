using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company;
using LT.DigitalOffice.CompanyService.Validation.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Validation.Helper;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;

namespace LT.DigitalOffice.CompanyService.Validation.Company
{
  public class EditCompanyRequestValidator : BaseEditRequestValidator<EditCompanyRequest>, IEditCompanyRequestValidator
  {
    private readonly List<string> imageFormats = new()
    {
      ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tga"
    };

    private void HandleInternalPropertyValidation(Operation<EditCompanyRequest> requestedOperation, CustomContext context)
    {
      Context = context;
      RequestedOperation = requestedOperation;

      #region Paths

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
          nameof(EditCompanyRequest.Logo),
          nameof(EditCompanyRequest.IsDepartmentModuleEnabled)
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
      AddСorrectOperations(nameof(EditCompanyRequest.IsDepartmentModuleEnabled), new List<OperationType> { OperationType.Replace });

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

      #region IsDepartmentModuleEnabled

      AddFailureForPropertyIf(
          nameof(EditCompanyRequest.IsDepartmentModuleEnabled),
          x => x == OperationType.Replace,
          new()
          {
            { x => bool.TryParse(x.value.ToString(), out bool _), "Incorrect format of IsDepartmentModuleEnabled." },
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
                  imageFormats.Contains(image.Extension))
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
