using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
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
        public Position GetPositionById([FromServices] IGetPositionByIdCommand command, [FromQuery] Guid positionId)
        {
            return command.Execute(positionId);
        }

        [HttpGet("getPositionsList")]
        public List<Position> GetPositionsList([FromServices] IGetPositionsListCommand command)
        {
            return command.Execute();
        }

        [HttpPost("addPosition")]
        public Guid AddPosition([FromServices] IAddPositionCommand command, [FromBody] PositionInfo request)
        {
            return command.Execute(request);
        }

        [HttpDelete("disablePositionById")]
        public void DisablePositionById([FromServices] IDisablePositionByIdCommand command, [FromQuery] Guid positionId)
        {
            command.Execute(positionId);
        }

        [HttpPost("editPosition")]
        public bool EditPosition([FromServices] IEditPositionCommand command, [FromBody] EditPositionRequest request)
        {
            return command.Execute(request);
        }
    }
}