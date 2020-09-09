using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto;

namespace LT.DigitalOffice.CompanyService.Validation
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