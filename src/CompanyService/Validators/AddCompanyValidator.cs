using FluentValidation;
using LT.DigitalOffice.CompanyService.Models;

namespace LT.DigitalOffice.CompanyService.Validators
{
    public class AddCompanyValidator : AbstractValidator<AddCompanyRequest>
    {
        public AddCompanyValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Company name is too long");
        }
    }
}