using System;
using System.Collections.Generic;
using FluentValidation;
using LT.DigitalOffice.CompanyService.Broker.Requests.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser;
using LT.DigitalOffice.CompanyService.Validation.CompanyUser.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation.CompanyUser
{
  public class CreateCompanyUserRequestValidator : AbstractValidator<CreateCompanyUserRequest>, ICreateCompanyUserRequestValidator
  {
    public CreateCompanyUserRequestValidator(
      ICompanyRepository companyRepository,
      ICompanyUserRepository companyUserRepository,
      IContractSubjectRepository contractRepository,
      IUserService userService)
    {
      RuleFor(x => x.UserId)
        .MustAsync(async (userId, _) => !await companyUserRepository.DoesExistAsync(userId))
        .WithMessage("User already exists.")
        .DependentRules(() =>
        {
          When(x => x.Rate.HasValue, () =>
          {
            RuleFor(x => x.Rate)
              .Must(x => x.Value > 0 && x.Value <= 1)
              .WithMessage("Rate must be from 0 to 1.");
          });

          RuleFor(x => x.ContractTermType)
            .IsInEnum();

          RuleFor(x => x.CompanyId)
            .MustAsync(async (request, _) => await companyRepository.DoesExistAsync())
            .WithMessage("Company does not exist.");

          When(x => x.ContractSubjectId.HasValue, () =>
          {
            RuleFor(x => x.ContractSubjectId)
              .MustAsync(async (request, _) => await contractRepository.DoesExistAsync(request.Value))
              .WithMessage("Contract subject does not exist.");
          });

          RuleFor(x => x.UserId)
            .MustAsync(async (request, _) => (await userService.CheckUsersExistence(new List<Guid> { request }, new List<string>())).Count == 1)
            .WithMessage("User does not exist.");
        });
    }
  }
}
