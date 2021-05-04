using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation
{
    public class PositionInfoValidator : AbstractValidator<PositionInfo>, IPositionInfoValidator
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