using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;

namespace LT.DigitalOffice.CompanyService.Validation.ModelValidators
{
    public class PositionInfoValidator : AbstractValidator<Position>
    {
        public PositionInfoValidator()
        {
            RuleFor(position => position.Name)
                .NotEmpty()
                .MaximumLength(80);

            RuleFor(position => position.Description)
                .NotEmpty()
                .MaximumLength(350);
        }
    }
}