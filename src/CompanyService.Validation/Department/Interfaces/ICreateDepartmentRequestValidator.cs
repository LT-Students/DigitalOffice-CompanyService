using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Validation.Department.Interfaces
{
    [AutoInject]
    public interface ICreateDepartmentRequestValidator : IValidator<CreateDepartmentRequest>
    {
    }
}
