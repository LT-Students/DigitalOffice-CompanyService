using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using LT.DigitalOffice.CompanyService.Validation.Office.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation.Office
{
    public class CreateOfficeRequestValidator : AbstractValidator<CreateOfficeRequest>, ICreateOfficeRequestValidator
    {
        public CreateOfficeRequestValidator()
        {
            RuleFor(request => request.City)
                .NotEmpty();

            RuleFor(request => request.Address)
                .NotEmpty();

            When(request => request.Name != null, () =>
            {
                RuleFor(request => request.Name)
                    .NotEmpty();
            });
        }
    }
}
