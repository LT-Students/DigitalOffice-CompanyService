using LT.DigitalOffice.CompanyService.Models.Dto;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Position
{
    /// <inheritdoc cref="IFindPositionsCommand"/>
    public class FindPositionsCommand : IFindPositionsCommand
    {
        private readonly IPositionRepository _repository;
        private readonly IPositionResponseMapper _mapper;

        public FindPositionsCommand(
            IPositionRepository repository,
            IPositionResponseMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public List<PositionResponse> Execute()
        {
            return _repository.Find().Select(position => _mapper.Map(position)).ToList();
        }
    }
}