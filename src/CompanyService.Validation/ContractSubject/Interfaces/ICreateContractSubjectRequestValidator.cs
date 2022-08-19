using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Validation.ContractSubject.Interfaces
{
  [AutoInject]
  public interface ICreateContractSubjectRequestValidator : IValidator<CreateContractSubjectRequest>
  {
  }
}
