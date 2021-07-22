using System.Linq;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Enums;

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

        public FindResultResponse<PositionInfo> Execute(int skipCount, int takeCount, bool includeDeactivated)
        {
            return new()
            {
                Status = OperationResultStatusType.FullSuccess,
                Body = _repository.Find(skipCount, takeCount, includeDeactivated, out int totalCount).Select(_mapper.Map).ToList(),
                TotalCount = totalCount
            };
        }
    }
}