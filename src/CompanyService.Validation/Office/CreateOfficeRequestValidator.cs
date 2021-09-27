using System.Text.RegularExpressions;
using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using LT.DigitalOffice.CompanyService.Validation.Office.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation.Office
{
  public class CreateOfficeRequestValidator : AbstractValidator<CreateOfficeRequest>, ICreateOfficeRequestValidator
  {
    public CreateOfficeRequestValidator()
    {
      RuleFor(request => request.Address.Trim())
        .NotEmpty().WithMessage("Name cannot be empty.");

      RuleFor(request => request.City.Trim())
        .NotEmpty().WithMessage("City cannot be empty.");

      RuleFor(request => request.Address.Trim())
        .NotEmpty().WithMessage("Address cannot be empty.");
    }
  }
}
