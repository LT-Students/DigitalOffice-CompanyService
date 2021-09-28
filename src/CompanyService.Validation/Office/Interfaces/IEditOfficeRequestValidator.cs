using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Validation.Office.Interfaces
{
  [AutoInject]
  public interface IEditOfficeRequestValidator : IValidator<JsonPatchDocument<EditOfficeRequest>>
  {
  }
}
