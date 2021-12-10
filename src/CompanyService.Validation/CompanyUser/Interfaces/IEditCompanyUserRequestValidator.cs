using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Validation.CompanyUser.Interfaces
{
  [AutoInject]
  public interface IEditCompanyUserRequestValidator : IValidator<JsonPatchDocument<EditCompanyUserRequest>>
  {
  }
}
