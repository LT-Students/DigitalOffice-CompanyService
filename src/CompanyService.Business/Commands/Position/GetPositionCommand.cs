using System;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Position
{
    /// <inheritdoc cref="IGetPositionCommand"/>
    public class GetPositionCommand : IGetPositionCommand
    {
        private readonly IPositionRepository _repository;
        private readonly IPositionInfoMapper _mapper;

        public GetPositionCommand(
            IPositionRepository repository,
            IPositionInfoMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public OperationResultResponse<PositionInfo> Execute(Guid positionId)
        {
            return new()
            {
                Status = OperationResultStatusType.FullSuccess,
                Body = _mapper.Map(_repository.Get(positionId, null))
            };
        }
    }
}