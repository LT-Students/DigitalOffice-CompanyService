using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Validation.Company.Interfaces
{
  [AutoInject]
  public interface ICreateCompanyRequestValidator : IValidator<CreateCompanyRequest>
  {
  }
}
