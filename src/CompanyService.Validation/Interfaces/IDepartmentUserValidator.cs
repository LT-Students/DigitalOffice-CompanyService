using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Validation.Interfaces
{
    [AutoInject]
    public interface IDepartmentUserValidator : IValidator<DepartmentUser>
    {
    }
}
