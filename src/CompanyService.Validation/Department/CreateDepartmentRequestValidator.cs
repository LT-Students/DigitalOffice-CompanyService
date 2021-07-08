using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.CompanyService.Validation.Department.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation.Department
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
