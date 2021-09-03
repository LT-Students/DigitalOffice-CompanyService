using System.Linq;
using System.Net;
using LT.DigitalOffice.CompanyService.Business.Commands.Office.Interface;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Office
{
  public class FindOfficesCommand : IFindOfficesCommand
  {
    private readonly IOfficeRepository _officeRepository;
    private readonly IOfficeInfoMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FindOfficesCommand(
        IOfficeRepository officeRepository,
        IOfficeInfoMapper mapper,
        IHttpContextAccessor httpContextAccessor)
    {
      _officeRepository = officeRepository;
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
    }

    public FindResultResponse<OfficeInfo> Execute(int skipCount, int takeCount, bool? includeDeactivated)
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
        Body = _officeRepository.Find(skipCount, takeCount, includeDeactivated, out int totalCount).Select(o => _mapper.Map(o)).ToList(),
        TotalCount = totalCount
      };
    }
  }
}
