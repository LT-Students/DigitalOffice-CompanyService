using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Validation.Interfaces
{
    [AutoInject]
    public interface IEditCompanyRequestValidator : IValidator<JsonPatchDocument<EditCompanyRequest>>
    {
    }
}
