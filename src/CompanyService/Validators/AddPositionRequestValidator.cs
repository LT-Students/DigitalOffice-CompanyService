using FluentValidation;
using LT.DigitalOffice.CompanyService.Models;

namespace LT.DigitalOffice.CompanyService.Validators
{
    public class AddPositionRequestValidator : AbstractValidator<AddPositionRequest>
    {
        public AddPositionRequestValidator()
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