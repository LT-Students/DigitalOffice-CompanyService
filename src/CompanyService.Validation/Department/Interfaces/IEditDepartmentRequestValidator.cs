using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Validation.Department.Interfaces
{
    [AutoInject]
    public interface IEditDepartmentRequestValidator : IValidator<JsonPatchDocument<EditDepartmentRequest>>
    {
    }
}
