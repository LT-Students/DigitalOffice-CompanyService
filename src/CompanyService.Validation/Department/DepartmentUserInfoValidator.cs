using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Validation.Department.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation.Department
{
    public class DepartmentUserInfoValidator : AbstractValidator<DepartmentUserInfo>, IDepartmentUserInfoValidator
    {
        public DepartmentUserInfoValidator()
        {
            RuleFor(du => du.UserId)
                .NotEmpty();

            RuleFor(du => du.PositionId)
                .NotEmpty();
        }

    }
}
