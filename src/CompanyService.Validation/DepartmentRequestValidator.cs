using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation
{
    public class DepartmentRequestValidator : AbstractValidator<NewDepartmentRequest>, IDepartmentRequestValidator
    {
        public DepartmentRequestValidator()
        {
            When(request => request.Users != null, () =>
            {
                RuleFor(request => request.Users)
                    .NotEmpty()
                    .ForEach(user =>
                    {
                        user.NotEmpty().SetValidator(new DepartmentUserValidator());
                    });
            });

            RuleFor(request => request.Info)
                .NotEmpty()
                .SetValidator(new DepartmentValidator());
        }
    }
}
