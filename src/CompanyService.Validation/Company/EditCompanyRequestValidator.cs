using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Validators;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company;
using LT.DigitalOffice.CompanyService.Validation.Company.Interfaces;
using LT.DigitalOffice.Kernel.Validators;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;

namespace LT.DigitalOffice.CompanyService.Validation.Company
{
  public class EditCompanyRequestValidator : ExtendedEditRequestValidator<Guid, EditCompanyRequest>, IEditCompanyRequestValidator
  {
    private readonly ICompanyRepository _companyRepository;
    private readonly IImageContentValidator _imageContentValidator;
    private readonly IImageExtensionValidator _imageExtensionValidator;

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
            { x => !string.IsNullOrEmpty(x.value?.ToString()), "CompanyName can't be empty." },
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
              ImageConsist image = JsonConvert.DeserializeObject<ImageConsist>(x.value?.ToString());

              return image is null
                ? true
                : _imageContentValidator.Validate(image.Content).IsValid &&
                  _imageExtensionValidator.Validate(image.Extension).IsValid;
            },
            "Incorrect Image format"
          }
        });

      #endregion
    }

    public EditCompanyRequestValidator(
      ICompanyRepository companyRepository,
      IImageContentValidator imageContentValidator,
      IImageExtensionValidator imageExtensionValidator)
    {
      _companyRepository = companyRepository;
      _imageContentValidator = imageContentValidator;
      _imageExtensionValidator = imageExtensionValidator;

      RuleFor(x => x.Item1)
        .MustAsync(async (companyId, _) => await _companyRepository.DoesExistAsync(companyId))
        .WithMessage("Company doesn't exist.");

      RuleForEach(x => x.Item2.Operations)
        .Custom(HandleInternalPropertyValidation);
    }
  }
}
