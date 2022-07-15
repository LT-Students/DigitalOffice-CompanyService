using FluentValidation;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject;
using LT.DigitalOffice.CompanyService.Validation.ContractSubject.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation.ContractSubject
{
  public class CreateContractSubjectRequestValidator : AbstractValidator<CreateContractSubjectRequest>, ICreateContractSubjectRequestValidator
  {
    public CreateContractSubjectRequestValidator(
      IContractSubjectRepository contractSubjectRepository)
    {
      RuleFor(request => request.Name)
        .MaximumLength(150).WithMessage("Name is too long.")
        .MustAsync(async (name, _) => !await contractSubjectRepository.DoesNameExistAsync(name))
        .WithMessage("Еhis Name already exists.");
    }
  }
}
