using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation
{
    public class DepartmentValidator : AbstractValidator<Department>, IDepartmentValidator
    {
        public DepartmentValidator()
        {
            When(request => request.DirectorUserId != null, () =>
            {
                RuleFor(request => request.DirectorUserId)
                    .NotEmpty().WithMessage("Director can not be empty.");
            });

            RuleFor(request => request.Name)
                .NotEmpty().WithMessage("Department name can not be empty.")
                .MinimumLength(2).WithMessage("Department name is too short")
                .MaximumLength(100).WithMessage("Department name is too long.");

            RuleFor(request => request.Description)
                .NotEmpty().WithMessage("Department description can not be empty.")
                .MaximumLength(1000).WithMessage("Department description is too long.");
        }
    }
}
