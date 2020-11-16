using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.ModelValidators;

namespace LT.DigitalOffice.CompanyService.Validation.RequestValidators
{
    public class DepartmentRequestValidator : AbstractValidator<NewDepartmentRequest>
    {
        public DepartmentRequestValidator()
        {
            RuleForEach(request => request.UsersIds)
                .NotEmpty();

            RuleFor(request => request.Info)
                .SetValidator(new DepartmentValidator());
        }
    }
}
