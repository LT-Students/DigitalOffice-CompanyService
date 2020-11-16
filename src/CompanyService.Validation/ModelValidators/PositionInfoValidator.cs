using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;

namespace LT.DigitalOffice.CompanyService.Validation.ModelValidators
{
    public class PositionInfoValidator : AbstractValidator<PositionInfo>
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