using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        [HttpGet("get")]
        public PositionResponse Get([FromServices] IGetPositionByIdCommand command, [FromQuery] Guid positionId)
        {
            return command.Execute(positionId);
        }

        [HttpGet("find")]
        public List<PositionResponse> Find([FromServices] IFindPositionsCommand command)
        {
            return command.Execute();
        }

        [HttpPost("create")]
        public Guid Create([FromServices] ICreatePositionCommand command, [FromBody] CreatePositionRequest request)
        {
            return command.Execute(request);
        }

        [HttpDelete("disable")]
        public void Disable([FromServices] IDisablePositionByIdCommand command, [FromQuery] Guid positionId)
        {
            command.Execute(positionId);
        }

        [HttpPost("edit")]
        public bool Edit([FromServices] IEditPositionCommand command, [FromBody] PositionInfo request)
        {
            return command.Execute(request);
        }
    }
}