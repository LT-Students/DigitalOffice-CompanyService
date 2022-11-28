using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Broker.Requests.Interfaces;
using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Models.Office;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company
{
  public class GetCompanyCommand : IGetCompanyCommand
  {
    private readonly ICompanyRepository _repository;
    private readonly ICompanyResponseMapper _mapper;
    private readonly IOfficeService _officeService;
    private readonly IResponseCreator _responseCreator;

    public GetCompanyCommand(
      ICompanyRepository repository,
      ICompanyResponseMapper mapper,
      IOfficeService officeService,
      IResponseCreator responseCreator)
    {
      _repository = repository;
      _mapper = mapper;
      _officeService = officeService;
      _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<CompanyResponse>> ExecuteAsync(GetCompanyFilter filter)
    {
      DbCompany company = await _repository.GetAsync();

      if (company is null)
      {
        return _responseCreator.CreateFailureResponse<CompanyResponse>(HttpStatusCode.NotFound);
      }

      OperationResultResponse<CompanyResponse> response = new();

      List<OfficeData> offices = filter.IncludeOffices
        ? await _officeService.GetOfficesAsync(response.Errors)
        : null;

      response.Body = _mapper.Map(company, offices, filter);

      return response;
    }
  }
}
