using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;

namespace LT.DigitalOffice.CompanyService.Validation
{
    public class DepartmentUserValidator : AbstractValidator<DepartmentUser>
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
