using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation
{
    public class CreatePositionRequestValidator : AbstractValidator<CreatePositionRequest>, ICreatePositionRequestValidator
    {
        public CreatePositionRequestValidator()
        {
            RuleFor(position => position.Name)
                .NotEmpty()
                .MaximumLength(80);

            When(position => !string.IsNullOrEmpty(position.Description), () =>
            {
                RuleFor(position => position.Description)
                    .NotEmpty()
                    .MaximumLength(350);
            });
        }
    }
}