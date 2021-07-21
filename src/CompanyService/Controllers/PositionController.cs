using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Position;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace LT.DigitalOffice.CompanyService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PositionController(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("get")]
        public OperationResultResponse<PositionResponse> Get(
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
            var result = command.Execute(request);

            if (result.Status == OperationResultStatusType.Conflict)
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;

                return result;
            }

            if (result.Status != OperationResultStatusType.Failed)
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
            }

            return result;
        }

        [HttpPatch("edit")]
        public OperationResultResponse<bool> Edit(
            [FromServices] IEditPositionCommand command,
            [FromQuery] Guid positionId,
            [FromBody] JsonPatchDocument<EditPositionRequest> request)
        {
            var result = command.Execute(positionId, request);

            if (result.Status == OperationResultStatusType.Conflict)
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
            }

            return result;
        }
    }
}