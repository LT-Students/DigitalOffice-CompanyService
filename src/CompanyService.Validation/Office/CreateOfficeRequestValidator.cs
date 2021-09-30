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
        .NotEmpty().WithMessage("Name must not be empty.");

      RuleFor(request => request.City)
        .NotEmpty().WithMessage("City must not be empty.");

      RuleFor(request => request.Address)
        .NotEmpty().WithMessage("Address must not be empty.");
    }
  }
}
