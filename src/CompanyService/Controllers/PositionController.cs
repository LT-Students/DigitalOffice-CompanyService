using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        [HttpGet("getPositionById")]
        public PositionResponse GetPositionById([FromServices] IGetPositionByIdCommand command, [FromQuery] Guid positionId)
        {
            return command.Execute(positionId);
        }

        [HttpGet("getPositionsList")]
        public List<PositionResponse> GetPositionsList([FromServices] IGetPositionsListCommand command)
        {
            return command.Execute();
        }

        [HttpPost("create")]
        public Guid CreatePosition([FromServices] ICreatePositionCommand command, [FromBody] Position request)
        {
            return command.Execute(request);
        }

        [HttpDelete("disablePositionById")]
        public void DisablePositionById([FromServices] IDisablePositionByIdCommand command, [FromQuery] Guid positionId)
        {
            command.Execute(positionId);
        }

        [HttpPost("editPosition")]
        public bool EditPosition([FromServices] IEditPositionCommand command, [FromBody] Position request)
        {
            return command.Execute(request);
        }
    }
}