using System;
using System.Collections.Generic;
using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Company.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation.Company
{
  public class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest>, ICreateCompanyRequestValidator
  {
    private readonly List<string> imageFormats = new()
    {
      ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tga"
    };

    public CreateCompanyRequestValidator()
    {
      RuleFor(request => request.PortalName)
        .NotEmpty()
        .WithMessage("Portal name can't be empty");

      RuleFor(request => request.CompanyName)
        .NotEmpty()
        .WithMessage("Company name can't be empty");

      RuleFor(request => request.SiteUrl)
        .NotEmpty()
        .WithMessage("Site url can't be empty");

      RuleFor(request => request.AdminInfo)
        .NotNull()
        .WithMessage("Admin information can't be null");

      RuleFor(request => request.SmtpInfo)
        .NotNull()
        .WithMessage("Smtp information can't be null");

      RuleFor(request => request.WorkDaysApiUrl)
        .NotEmpty()
        .WithMessage("Work days api url can't be empty");

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
          .Must(imageFormats.Contains)
          .WithMessage($"Image extension is not {string.Join('/', imageFormats)}");
      });
    }
  }
}
