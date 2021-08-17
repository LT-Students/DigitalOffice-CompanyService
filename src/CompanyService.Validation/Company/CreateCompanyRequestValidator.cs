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
                .WithMessage("PortalName can't be empty");

            RuleFor(request => request.CompanyName)
                .NotEmpty()
                .WithMessage("CompanyName can't be empty");

            RuleFor(request => request.SiteUrl)
                .NotEmpty()
                .WithMessage("SiteUrl can't be empty");

            RuleFor(request => request.AdminInfo)
                .NotNull()
                .WithMessage("AdminInfo can't be null");

            RuleFor(request => request.SmtpInfo)
                .NotNull()
                .WithMessage("SmtpInfo can't be null");

            RuleFor(request => request.WorkDaysApiUrl)
                .NotEmpty()
                .WithMessage("WorkDaysApiUrl can't be empty");
        }
    }
}
