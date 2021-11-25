using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Validation.CompanyUser.Interfaces
{
  [AutoInject]
  public interface IEditCompanyUserRequestValidator : IValidator<EditCompanyUserRequest>
  {
  }
}
