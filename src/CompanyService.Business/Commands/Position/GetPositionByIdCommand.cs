using LT.DigitalOffice.CompanyService.Models.Dto;
using System;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Position
{
    /// <inheritdoc cref="IGetPositionByIdCommand"/>
    public class GetPositionByIdCommand : IGetPositionByIdCommand
    {
        private readonly IPositionRepository _repository;
        private readonly IPositionResponseMapper _mapper;

        public GetPositionByIdCommand(
            IPositionRepository repository,
            IPositionResponseMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public PositionResponse Execute(Guid positionId)
        {
            var dbPosition = _repository.Get(positionId, null);
            return _mapper.Map(dbPosition);
        }
    }
}