using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Broker.Requests.Interfaces;
using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Models.Office;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company
{
  public class GetCompanyCommand : IGetCompanyCommand
  {
    private readonly ICompanyRepository _repository;
    private readonly ICompanyResponseMapper _mapper;
    private readonly IOfficeService _officeService;

    public GetCompanyCommand(
      ICompanyRepository repository,
      ICompanyResponseMapper mapper,
      IOfficeService officeService)
    {
      _repository = repository;
      _mapper = mapper;
      _officeService = officeService;
    }

    public async Task<OperationResultResponse<CompanyResponse>> ExecuteAsync(GetCompanyFilter filter)
    {
      List<string> errors = new();
      DbCompany company = await _repository.GetAsync();

      List<OfficeData> offices = filter.IncludeOffices
        ? await _officeService.GetOfficesAsync(errors)
        : null;

      return new OperationResultResponse<CompanyResponse>
      {
        Status = errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess,
        Body = _mapper.Map(company, offices, filter),
        Errors = errors
      };
    }
  }
}
