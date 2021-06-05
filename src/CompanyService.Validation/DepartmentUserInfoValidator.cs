using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation
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
