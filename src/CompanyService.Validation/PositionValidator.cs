using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;

namespace LT.DigitalOffice.CompanyService.Validation
{
    public class PositionValidator : AbstractValidator<Position>
    {
        public PositionValidator()
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