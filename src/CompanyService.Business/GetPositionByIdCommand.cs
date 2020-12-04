using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using LT.DigitalOffice.CompanyService.Data.Interfaces;

namespace LT.DigitalOffice.CompanyService.Business
{
    /// <inheritdoc cref="IGetPositionByIdCommand"/>
    public class GetPositionByIdCommand : IGetPositionByIdCommand
    {
        private readonly IPositionRepository repository;
        private readonly IMapper<DbPosition, PositionResponse> mapper;

        public GetPositionByIdCommand(
            [FromServices] IPositionRepository repository,
            [FromServices] IMapper<DbPosition, PositionResponse> mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public PositionResponse Execute(Guid positionId)
        {
            var dbPosition = repository.GetPositionById(positionId);
            return mapper.Map(dbPosition);
        }
    }
}