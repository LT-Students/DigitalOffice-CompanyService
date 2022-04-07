using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Models.Office;
using LT.DigitalOffice.Models.Broker.Requests.Office;
using LT.DigitalOffice.Models.Broker.Responses.Office;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company
{
  public class GetCompanyCommand : IGetCompanyCommand
  {
    private readonly ICompanyRepository _repository;
    private readonly ICompanyResponseMapper _companyResponseMapper;
    private readonly IRequestClient<IGetOfficesRequest> _rcGetOffices;
    private readonly ILogger<GetCompanyCommand> _logger;

    private async Task<List<OfficeData>> GetOfficesThroughBrokerAsync(
      List<string> errors)
    {
      const string errorMessage = "Can not get office info. Please try again later.";

      try
      {
        Response<IOperationResult<IGetOfficesResponse>> response = await _rcGetOffices
          .GetResponse<IOperationResult<IGetOfficesResponse>>(
            IGetOfficesRequest.CreateObj(null));

        if (response.Message.IsSuccess)
        {
          _logger.LogInformation("Offices were taken from the service.");

          return response.Message.Body.Offices;
        }
        else
        {
          _logger.LogWarning("Errors while getting departments info. Reason: {Errors}",
            string.Join('\n', response.Message.Errors));
        }
      }
      catch (Exception exc)
      {
        _logger.LogError(exc, errorMessage);
      }

      errors.Add(errorMessage);

      return null;
    }

    public GetCompanyCommand(
      ICompanyRepository repository,
      ICompanyResponseMapper mapper,
      IRequestClient<IGetOfficesRequest> rcGetOffices,
      ILogger<GetCompanyCommand> logger)
    {
      _repository = repository;
      _companyResponseMapper = mapper;
      _rcGetOffices = rcGetOffices;
      _logger = logger;
    }

    public async Task<OperationResultResponse<CompanyResponse>> ExecuteAsync(Guid companyId, GetCompanyFilter filter)
    {
      List<string> errors = new();
      DbCompany company = await _repository.GetAsync(companyId);

      List<OfficeData> offices = filter.IncludeOffices
        ? await GetOfficesThroughBrokerAsync(errors)
        : null;

      return new OperationResultResponse<CompanyResponse>
      {
        Status = errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess,
        Body = _companyResponseMapper.Map(company, offices, filter)
      };
    }
  }
}
