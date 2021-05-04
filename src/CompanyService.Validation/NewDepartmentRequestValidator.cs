using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation
{
    public class NewDepartmentRequestValidator : AbstractValidator<NewDepartmentRequest>, INewDepartmentRequestValidator
    {
        public NewDepartmentRequestValidator()
        {
            When(request => request.Users != null, () =>
            {
                RuleFor(request => request.Users)
                    .NotEmpty()
                    .ForEach(user =>
                    {
                        user.NotEmpty().SetValidator(new DepartmentUserInfoValidator());
                    });
            });

            RuleFor(request => request.Info)
                .NotEmpty()
                .SetValidator(new BaseDepartmentInfoValidator());
        }
    }
}
