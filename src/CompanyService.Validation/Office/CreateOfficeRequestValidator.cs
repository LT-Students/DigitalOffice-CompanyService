using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using LT.DigitalOffice.CompanyService.Validation.Office.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation.Office
{
  public class CreateOfficeRequestValidator : AbstractValidator<CreateOfficeRequest>, ICreateOfficeRequestValidator
  {
    public CreateOfficeRequestValidator()
    {
      RuleFor(request => request.City)
        .Must(c => !string.IsNullOrEmpty(c?.Trim())).WithMessage("City must not be empty.");

      RuleFor(request => request.Address)
        .Must(a => !string.IsNullOrEmpty(a?.Trim())).WithMessage("Address must not be empty.");
    }
  }
}
