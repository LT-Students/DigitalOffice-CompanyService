using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Company.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation.Company
{
    public class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest>, ICreateCompanyRequestValidator
    {
        public CreateCompanyRequestValidator()
        {
            RuleFor(request => request.PortalName)
                .NotEmpty();

            RuleFor(request => request.CompanyName)
                .NotEmpty();

            RuleFor(request => request.SiteUrl)
                .NotEmpty();
        }
    }
}
