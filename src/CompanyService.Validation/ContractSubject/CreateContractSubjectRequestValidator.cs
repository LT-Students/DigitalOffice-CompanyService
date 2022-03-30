using FluentValidation;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject;
using LT.DigitalOffice.CompanyService.Validation.ContractSubject.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation.ContractSubject
{
  public class CreateContractSubjectRequestValidator : AbstractValidator<CreateContractSubjectRequest>, ICreateContractSubjectRequestValidator
  {
    private readonly ICompanyRepository _companyRepository;
    public CreateContractSubjectRequestValidator(
      ICompanyRepository companyRepository)
    {
      RuleFor(request => request.CompanyId)
        .MustAsync(async (companyId, _) => !await _companyRepository.DoesCompanyExistAsync(companyId))
        .WithMessage("Company doesn't exist.");

      RuleFor(request => request.Name)
        .NotEmpty().WithMessage("Name can't be empty.");
    }
  }
}
