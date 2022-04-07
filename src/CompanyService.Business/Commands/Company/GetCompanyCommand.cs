using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Models.Department;
using LT.DigitalOffice.Models.Broker.Models.Office;
using LT.DigitalOffice.Models.Broker.Models.Position;
using LT.DigitalOffice.Models.Broker.Requests.Department;
using LT.DigitalOffice.Models.Broker.Requests.Office;
using LT.DigitalOffice.Models.Broker.Requests.Position;
using LT.DigitalOffice.Models.Broker.Responses.Department;
using LT.DigitalOffice.Models.Broker.Responses.Office;
using LT.DigitalOffice.Models.Broker.Responses.Position;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company
{
  public class GetCompanyCommand : IGetCompanyCommand
  {
    private readonly ICompanyRepository _repository;
    private readonly ICompanyInfoMapper _companyInfoMapper;
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
      ICompanyInfoMapper mapper,
      IRequestClient<IGetOfficesRequest> rcGetOffices,
      ILogger<GetCompanyCommand> logger)
    {
      _repository = repository;
      _companyInfoMapper = mapper;
      _rcGetOffices = rcGetOffices;
      _logger = logger;
    }

    public async Task<OperationResultResponse<CompanyInfo>> ExecuteAsync(Guid companyId, GetCompanyFilter filter)
    {
      List<string> errors = new();
      DbCompany company = await _repository.GetAsync(companyId);

      List<OfficeData> offices = filter.IncludeOffices
        ? await GetOfficesThroughBrokerAsync(errors)
        : null;

      return new OperationResultResponse<CompanyInfo>
      {
        Status = errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess,
        Body = _companyInfoMapper.Map(company, offices, filter)
      };
    }
  }
}
