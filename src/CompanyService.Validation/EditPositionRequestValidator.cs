using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.ModelValidators;

namespace LT.DigitalOffice.CompanyService.Validation
{
    public class EditPositionRequestValidator : AbstractValidator<EditPositionRequest>
    {
        public EditPositionRequestValidator()
        {
            RuleFor(request => request.Id)
                .NotEmpty();

            RuleFor(request => request.Info)
                .SetValidator(new PositionInfoValidator());
        }
    }
}