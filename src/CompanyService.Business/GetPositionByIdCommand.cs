using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto;
using System;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces;

namespace LT.DigitalOffice.CompanyService.Business
{
    /// <inheritdoc cref="IGetPositionByIdCommand"/>
    public class GetPositionByIdCommand : IGetPositionByIdCommand
    {
        private readonly IPositionRepository repository;
        private readonly IPositionMapper mapper;

        public GetPositionByIdCommand(
            IPositionRepository repository,
            IPositionMapper mapper)
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