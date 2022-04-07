using System;
using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Validation.Company.Interfaces
{
  [AutoInject]
  public interface IEditCompanyRequestValidator : IValidator<(Guid, JsonPatchDocument<EditCompanyRequest>)>
  {
  }
}
