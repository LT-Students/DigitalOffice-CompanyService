using FluentValidation;
using LT.DigitalOffice.CompanyService.Models;

namespace LT.DigitalOffice.CompanyService.Validators
{
    public class EditPositionRequestValidator : AbstractValidator<EditPositionRequest>
    {
        public EditPositionRequestValidator()
        {
            RuleFor(position => position.Id)
                .NotEmpty();

            RuleFor(position => position.Name)
                .NotEmpty()
                .MaximumLength(80)
                .WithMessage("Position name is too long");

            RuleFor(position => position.Description)
                .NotEmpty()
                .MaximumLength(350)
                .WithMessage("Position description is too long");
        }
    }
}