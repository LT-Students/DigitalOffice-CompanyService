using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Position;
using LT.DigitalOffice.CompanyService.Validation.Position.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation.Position
{
    public class CreatePositionRequestValidator : AbstractValidator<CreatePositionRequest>, ICreatePositionRequestValidator
    {
        public CreatePositionRequestValidator()
        {
            RuleFor(position => position.Name)
                .NotEmpty()
                .MaximumLength(80);

            When(position => position.Description != null, () =>
            {
                RuleFor(position => position.Description)
                    .NotEmpty()
                    .MaximumLength(350);
            });
        }
    }
}