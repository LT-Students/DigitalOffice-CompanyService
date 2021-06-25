using FluentValidation;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation
{
    public class CreateOfficeRequestValidator : AbstractValidator<CreateOfficeRequest>, ICreateOfficeRequestValidator
    {
        public CreateOfficeRequestValidator(
            ICompanyRepository companyRepository)
        {
            RuleFor(request => request.CompanyId)
                .Must(id => companyRepository.Get().Id == id)
                .WithMessage("Wrong company id");

            RuleFor(request => request.City)
                .NotEmpty();

            RuleFor(request => request.Address)
                .NotEmpty();
        }
    }
}
