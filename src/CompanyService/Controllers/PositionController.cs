using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Position;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.CompanyService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        [HttpGet("get")]
        public OperationResultResponse<PositionInfo> Get(
            [FromServices] IGetPositionCommand command,
            [FromQuery] Guid positionId)
        {
            return command.Execute(positionId);
        }

        [HttpGet("find")]
        public FindResultResponse<PositionInfo> Find(
            [FromServices] IFindPositionsCommand command,
            [FromQuery] int skipCount,
            [FromQuery] int takeCount,
            [FromQuery] bool includeDeactivated = false)
        {
            return command.Execute(skipCount, takeCount, includeDeactivated);
        }

        [HttpPost("create")]
        public OperationResultResponse<Guid> Create(
            [FromServices] ICreatePositionCommand command,
            [FromBody] CreatePositionRequest request)
        {
            return command.Execute(request);
        }

        [HttpPatch("edit")]
        public OperationResultResponse<bool> Edit(
            [FromServices] IEditPositionCommand command,
            [FromQuery] Guid positionId,
            [FromBody] JsonPatchDocument<EditPositionRequest> request)
        {
            return command.Execute(positionId, request);
        }
    }
}