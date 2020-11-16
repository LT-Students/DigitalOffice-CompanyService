using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.ModelValidators;

namespace LT.DigitalOffice.CompanyService.Validation
{
    public class DepartmentRequestValidator : AbstractValidator<DepartmentRequest>
    {
        public DepartmentRequestValidator()
        {
            RuleForEach(request => request.UsersIds)
                .NotEmpty();

            RuleFor(request => request.Info)
                .SetValidator(new DepartmentInfoValidator());
        }
    }
}
