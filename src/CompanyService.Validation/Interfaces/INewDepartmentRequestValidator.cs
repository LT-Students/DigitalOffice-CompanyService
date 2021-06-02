using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Validation.Interfaces
{
    [AutoInject]
    public interface INewDepartmentRequestValidator : IValidator<CreateDepartmentRequest>
    {
    }
}
