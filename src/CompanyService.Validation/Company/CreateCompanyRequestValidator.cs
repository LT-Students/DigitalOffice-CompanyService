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
                .NotEmpty()
                .WithMessage("Portal name can't be empty");

            RuleFor(request => request.CompanyName)
                .NotEmpty()
                .WithMessage("Company name can't be empty");

            RuleFor(request => request.SiteUrl)
                .NotEmpty()
                .WithMessage("Site url can't be empty");

            RuleFor(request => request.AdminInfo)
                .NotNull()
                .WithMessage("Admin information can't be null");

            RuleFor(request => request.SmtpInfo)
                .NotNull()
                .WithMessage("Smtp information can't be null");

            RuleFor(request => request.WorkDaysApiUrl)
                .NotEmpty()
                .WithMessage("Work days api url can't be empty");
        }
    }
}
