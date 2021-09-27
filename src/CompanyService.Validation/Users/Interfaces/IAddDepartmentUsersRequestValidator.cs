using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.User;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Validation.Users.Interfaces
{
  [AutoInject]
  public interface IAddDepartmentUsersRequestValidator : IValidator<AddDepartmentUsersRequest>
  {
  }
}
