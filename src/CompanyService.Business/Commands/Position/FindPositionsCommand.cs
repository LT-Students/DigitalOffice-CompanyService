using System.Linq;
using System.Net;
using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Position
{
  /// <inheritdoc cref="IFindPositionsCommand"/>
  public class FindPositionsCommand : IFindPositionsCommand
  {
    private readonly IPositionRepository _repository;
    private readonly IPositionInfoMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FindPositionsCommand(
        IPositionRepository repository,
        IPositionInfoMapper mapper,
        IHttpContextAccessor httpContextAccessor)
    {
      _repository = repository;
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
    }

    public FindResultResponse<PositionInfo> Execute(int skipCount, int takeCount, bool includeDeactivated)
    {
      if (skipCount < 0)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return new()
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { "Skip count can't be less than 0." }
        };
      }

      if (takeCount < 1)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return new()
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { "Take count can't be less than 1." }
        };
      }

      return new()
      {
        Status = OperationResultStatusType.FullSuccess,
        Body = _repository.Find(skipCount, takeCount, includeDeactivated, out int totalCount).Select(_mapper.Map).ToList(),
        TotalCount = totalCount
      };
    }
  }
}
