using System;
using System.Net;
using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Position
{
  /// <inheritdoc cref="IGetPositionCommand"/>
  public class GetPositionCommand : IGetPositionCommand
  {
    private readonly IPositionRepository _repository;
    private readonly IPositionInfoMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetPositionCommand(
        IPositionRepository repository,
        IPositionInfoMapper mapper,
        IHttpContextAccessor httpContextAccessor)
    {
      _repository = repository;
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
    }

    public OperationResultResponse<PositionInfo> Execute(Guid positionId)
    {
      DbPosition position = _repository.Get(positionId);

      if (position == null)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;

        return new()
        {
          Status = OperationResultStatusType.FullSuccess,
          Errors = new() { $"Position with id: '{positionId}' doesn't exist."}
        };
      }

      return new()
      {
        Status = OperationResultStatusType.FullSuccess,
        Body = _mapper.Map(position)
      };
    }
  }
}
