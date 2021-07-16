using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Position;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;
using System;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces
{
    [AutoInject]
    public interface IEditPositionCommand
    {
        OperationResultResponse<bool> Execute(Guid positionId, JsonPatchDocument<EditPositionRequest> request);
    }
}