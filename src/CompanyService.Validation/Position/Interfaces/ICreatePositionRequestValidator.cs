using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Position;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Validation.Position.Interfaces
{
    [AutoInject]
    public interface ICreatePositionRequestValidator : IValidator<CreatePositionRequest>
    {
    }
}
