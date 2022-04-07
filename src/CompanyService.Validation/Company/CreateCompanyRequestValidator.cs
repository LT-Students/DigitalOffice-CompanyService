using FluentValidation;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Company.Interfaces;
using LT.DigitalOffice.Kernel.Validators.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation.Company
{
  public class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest>, ICreateCompanyRequestValidator
  {
    public CreateCompanyRequestValidator(
      ICompanyRepository _companyRepository,
      IImageContentValidator _imageContentValidator,
      IImageExtensionValidator _imageExtensionValidator)
    {
      // TODO rework for supprot more than one company

      RuleFor(request => request)
        .Cascade(CascadeMode.Stop)
        .MustAsync(async (x, _) => !await _companyRepository.DoesExistAsync())
        .WithMessage("Company already exists.")
        .ChildRules((request) =>
        { 
          RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage("Company name can't be empty.");

          When(w => w.Logo != null, () =>
          {
            RuleFor(w => w.Logo.Content)
              .SetValidator(_imageContentValidator);

            RuleFor(w => w.Logo.Extension)
              .SetValidator(_imageExtensionValidator);
          });
        });
    }
  }
}
