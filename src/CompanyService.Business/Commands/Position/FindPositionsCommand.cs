using System.Linq;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Position
{
    /// <inheritdoc cref="IFindPositionsCommand"/>
    public class FindPositionsCommand : IFindPositionsCommand
    {
        private readonly IPositionRepository _repository;
        private readonly IPositionInfoMapper _mapper;

        public FindPositionsCommand(
            IPositionRepository repository,
            IPositionInfoMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public FindPositionsResponse Execute(int skipCount, int takeCount, bool includeDeactivated)
        {
            return new()
            {
                Positions = _repository.Find(skipCount, takeCount, includeDeactivated, out int totalCount).Select(_mapper.Map).ToList(),
                TotalCount = totalCount
            };
        }
    }
}