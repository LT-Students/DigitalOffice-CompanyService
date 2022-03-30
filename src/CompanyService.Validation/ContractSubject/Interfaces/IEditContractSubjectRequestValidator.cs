using System;
using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Validation.ContractSubject.Interfaces
{
  [AutoInject]
  public interface IEditContractSubjectRequestValidator : IValidator<(Guid, JsonPatchDocument<EditContractSubjectRequest>)>
  {
  }
}
