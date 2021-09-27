using System.Text.RegularExpressions;
using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using LT.DigitalOffice.CompanyService.Validation.Office.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation.Office
{
  public class CreateOfficeRequestValidator : AbstractValidator<CreateOfficeRequest>, ICreateOfficeRequestValidator
  {
    private Regex NumberRegex = new(@"\d");
    private Regex SpecialCharactersRegex = new(@"[$&+,:;=?@#|<>.^*()%!]");
    private Regex NameRegex = new(@"^[a-zA-Zа-яА-ЯёЁ'][a-zA-Z-а-яА-ЯёЁ' ]+[a-zA-Zа-яА-ЯёЁ']?$");
    public CreateOfficeRequestValidator()
    {
      RuleFor(request => request.City)
        .NotEmpty().WithMessage("City name cannot be empty.")
        .Must(x => !NumberRegex.IsMatch(x))
        .WithMessage("City name must not contain numbers.")
        .Must(x => !SpecialCharactersRegex.IsMatch(x))
        .WithMessage("City name must not contain special characters, except dash.")
        .MaximumLength(32)
        .WithMessage("City name is too long.")
        .Must(x => NameRegex.IsMatch(x.Trim()))
        .WithMessage("City name contains invalid characters.");

      RuleFor(request => request.Address)
        .NotEmpty().WithMessage("Address cannot be empty.")
        .MaximumLength(100).WithMessage("Address is too long.");
    }
  }
}
