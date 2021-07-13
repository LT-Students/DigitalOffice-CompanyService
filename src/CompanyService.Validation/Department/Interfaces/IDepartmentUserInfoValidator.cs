using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Validation.Department.Interfaces
{
    [AutoInject]
    public interface IDepartmentUserInfoValidator : IValidator<DepartmentUserInfo>
    {
    }
}
