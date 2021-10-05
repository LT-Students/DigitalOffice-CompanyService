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

      RuleFor(request => request)
        .Must(requst => requst.LogoExtension != null && requst.LogoContent != null
          || requst.LogoExtension == null && requst.LogoContent == null)
        .WithMessage("Content and extension must both be filled");

      When(project => !string.IsNullOrEmpty(project.LogoContent?.Trim()), () =>
      {
        RuleFor(project => project.LogoContent)
          .Must(content =>
          {
            try
            {
              return Convert.TryFromBase64String(content, new Span<byte>(new byte[content.Length]), out _);
            }
            catch
            {
              return false;
            }
          }).WithMessage("Wrong image content.");
      });

      When(project => !string.IsNullOrEmpty(project.LogoExtension), () =>
      {
        RuleFor(project => project.LogoExtension)
          .Must(extension => imageFormats.Contains(extension));
      });
    }
  }
}
