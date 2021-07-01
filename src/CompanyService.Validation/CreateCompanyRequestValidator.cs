using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation
{
    public class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest>,  ICreateCompanyRequestValidator
    {
        public CreateCompanyRequestValidator()
        {
            When(request => request.PortalName != null, () =>
            {
                RuleFor(request => request.PortalName)
                    .NotEmpty();
            });

            RuleFor(request => request.CompanyName)
                .NotEmpty();

            RuleFor(request => request.SiteUrl)
                .NotEmpty();
        }
    }
}
