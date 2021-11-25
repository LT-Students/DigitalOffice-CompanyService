using FluentValidation;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser;
using LT.DigitalOffice.CompanyService.Validation.CompanyUser.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation.CompanyUser
{
  public class EditUserRateRequestValidator : AbstractValidator<EditCompanyUserRequest>, IEditCompanyUserRequestValidator
  {
    public EditUserRateRequestValidator(
      ICompanyUserRepository userRepository)
    {
      RuleFor(x => x.Rate)
        .LessThanOrEqualTo(1)
        .GreaterThan(0);

      RuleFor(x => x.UserId)
        .MustAsync(async (id, _) => await userRepository.DoesExistAsync(id))
        .WithMessage("User must exist.");
    }
  }
}
