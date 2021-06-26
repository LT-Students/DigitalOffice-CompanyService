using FluentValidation;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation
{
    public class CreateOfficeRequestValidator : AbstractValidator<CreateOfficeRequest>, ICreateOfficeRequestValidator
    {
        public CreateOfficeRequestValidator()
        {
            RuleFor(request => request.City)
                .NotEmpty();

            RuleFor(request => request.Address)
                .NotEmpty();
        }
    }
}
