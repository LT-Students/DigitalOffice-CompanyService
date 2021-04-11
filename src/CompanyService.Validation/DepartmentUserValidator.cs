using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation
{
    public class DepartmentUserValidator : AbstractValidator<DepartmentUser>, IDepartmentUserValidator
    {
        public DepartmentUserValidator()
        {
            RuleFor(du => du.UserId)
                .NotEmpty();

            RuleFor(du => du.PositionId)
                .NotEmpty();
        }

    }
}
