using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation
{
    public class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentRequest>, ICreateDepartmentRequestValidator
    {
        public CreateDepartmentRequestValidator()
        {
            When(request => request.Users != null, () =>
            {
                RuleForEach(request => request.Users)
                    .NotEmpty();
            });

            RuleFor(request => request.Info)
                .NotEmpty()
                .SetValidator(new BaseDepartmentInfoValidator());
        }
    }
}
