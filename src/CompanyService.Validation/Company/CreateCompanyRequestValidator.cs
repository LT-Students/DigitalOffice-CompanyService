using System;
using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Company.Interfaces;
using LT.DigitalOffice.Kernel.Constants;

namespace LT.DigitalOffice.CompanyService.Validation.Company
{
  public class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest>, ICreateCompanyRequestValidator
  {
    public CreateCompanyRequestValidator()
    {
      RuleFor(request => request.Name)
        .NotEmpty()
        .WithMessage("Company name can't be empty");

      When(w => w.Logo != null, () =>
      {
        RuleFor(w => w.Logo.Content)
          .NotEmpty().WithMessage("Image content cannot be empty.")
          .Must(x =>
          {
            try
            {
              var byteString = new Span<byte>(new byte[x.Length]);
              return Convert.TryFromBase64String(x, byteString, out _);
            }
            catch
            {
              return false;
            }
          }).WithMessage("Wrong image content.");

        RuleFor(w => w.Logo.Extension)
          .Must(x => ImageFormats.formats.Contains(x))
          .WithMessage($"Image extension is not {string.Join('/', ImageFormats.formats)}");
      });
    }
  }
}
