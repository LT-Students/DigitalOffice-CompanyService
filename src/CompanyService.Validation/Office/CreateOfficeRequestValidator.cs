using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using LT.DigitalOffice.CompanyService.Validation.Office.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation.Office
{
  public class CreateOfficeRequestValidator : AbstractValidator<CreateOfficeRequest>, ICreateOfficeRequestValidator
  {
    public CreateOfficeRequestValidator()
    {
      RuleFor(request => request.Name)
        .Must(n => !string.IsNullOrEmpty(n?.Trim())).WithMessage("Name must not be empty.");

      RuleFor(request => request.City)
        .Must(n => !string.IsNullOrEmpty(n?.Trim())).WithMessage("City must not be empty.");

      RuleFor(request => request.Address)
        .Must(n => !string.IsNullOrEmpty(n?.Trim())).WithMessage("Address must not be empty.");
    }
  }
}
