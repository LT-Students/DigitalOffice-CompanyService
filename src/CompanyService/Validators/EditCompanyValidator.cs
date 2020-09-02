using FluentValidation;
using LT.DigitalOffice.CompanyService.Models;

namespace LT.DigitalOffice.CompanyService.Validators
{
    public class EditCompanyValidator : AbstractValidator<EditCompanyRequest>
    {
        public EditCompanyValidator()
        {
            RuleFor(company => company.Name)
                .NotEqual("") // Must be null or not empty string.
                .MaximumLength(100)
                .WithMessage("Company name is too long");

            RuleFor(company => company.CompanyId)
                .NotEmpty();

            RuleFor(company => company.IsActive)
                .NotNull();
        }
    }
}
