using LT.DigitalOffice.CompanyService.Commands.Interfaces;
using LT.DigitalOffice.CompanyService.Database.Entities;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models;
using LT.DigitalOffice.CompanyService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Commands
{
    public class GetPositionsListCommand : IGetPositionsListCommand
    {
        private readonly IPositionRepository repository;
        private readonly IMapper<DbPosition, Position> mapper;

        public GetPositionsListCommand(
            [FromServices] IPositionRepository repository,
            [FromServices] IMapper<DbPosition, Position> mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public List<Position> Execute()
        {
            return repository.GetPositionsList().Select(position => mapper.Map(position)).ToList();
        }
    }
}