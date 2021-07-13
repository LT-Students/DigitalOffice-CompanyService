using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Position;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Validation.Position.Interfaces
{
    [AutoInject]
    public interface IEditPositionRequestValidator : IValidator<JsonPatchDocument<EditPositionRequest>>
    {
    }
}
