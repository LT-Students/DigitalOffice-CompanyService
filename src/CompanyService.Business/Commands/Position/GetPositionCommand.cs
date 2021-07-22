using LT.DigitalOffice.CompanyService.Models.Dto;
using System;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Enums;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Position
{
    /// <inheritdoc cref="IGetPositionCommand"/>
    public class GetPositionCommand : IGetPositionCommand
    {
        private readonly IPositionRepository _repository;
        private readonly IPositionResponseMapper _mapper;

        public GetPositionCommand(
            IPositionRepository repository,
            IPositionResponseMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public OperationResultResponse<PositionResponse> Execute(Guid positionId)
        {
            return new()
            {
                Status = OperationResultStatusType.FullSuccess,
                Body = _mapper.Map(_repository.Get(positionId, null))
            };
        }
    }
}