using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto;

namespace LT.DigitalOffice.CompanyService.Validation
{
    public class DepartmentRequestValidator : AbstractValidator<DepartmentRequest>
    {
        public DepartmentRequestValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty().WithMessage("Department name can not be empty.")
                .MinimumLength(2).WithMessage("Department name is too short")
                .MaximumLength(100).WithMessage("Department name is too long.");

            RuleFor(request => request.CompanyId)
                .NotEmpty().WithMessage("Company Id can not be empty.");
        }
    }
}
