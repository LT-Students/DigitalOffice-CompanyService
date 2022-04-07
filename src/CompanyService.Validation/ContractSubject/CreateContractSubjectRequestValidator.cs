using FluentValidation;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject;
using LT.DigitalOffice.CompanyService.Validation.ContractSubject.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation.ContractSubject
{
  public class CreateContractSubjectRequestValidator : AbstractValidator<CreateContractSubjectRequest>, ICreateContractSubjectRequestValidator
  {
    public CreateContractSubjectRequestValidator(
      IContractSubjectRepository _contractSubjectRepository)
    {
      RuleFor(request => request.Name)
        .NotEmpty().WithMessage("Name can't be empty.")
        .MaximumLength(150).WithMessage("Name is too long.")
        .MustAsync(async (name, _) => await _contractSubjectRepository.DoesNameExistAsync(name));
    }
  }
}
