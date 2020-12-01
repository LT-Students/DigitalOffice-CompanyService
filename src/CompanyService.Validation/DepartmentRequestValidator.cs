using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;

namespace LT.DigitalOffice.CompanyService.Validation
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
