using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Validation.Office.Interfaces
{
    [AutoInject]
    public interface ICreateOfficeRequestValidator : IValidator<CreateOfficeRequest>
    {
    }
}
