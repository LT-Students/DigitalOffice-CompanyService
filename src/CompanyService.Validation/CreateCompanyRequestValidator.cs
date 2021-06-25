using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation
{
    public class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest>,  ICreateCompanyRequestValidator
    {
        public CreateCompanyRequestValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty();
        }
    }
}
