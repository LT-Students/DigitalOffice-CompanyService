using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto;

namespace LT.DigitalOffice.CompanyService.Validation
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