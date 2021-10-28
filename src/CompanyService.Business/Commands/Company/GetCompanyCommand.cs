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
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Models.Department;
using LT.DigitalOffice.Models.Broker.Models.Position;
using LT.DigitalOffice.Models.Broker.Requests.Department;
using LT.DigitalOffice.Models.Broker.Requests.Position;
using LT.DigitalOffice.Models.Broker.Responses.Department;
using LT.DigitalOffice.Models.Broker.Responses.Position;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company
{
  public class GetCompanyCommand : IGetCompanyCommand
  {
    private readonly ICompanyRepository _repository;
    private readonly ICompanyInfoMapper _companyInfoMapper;
    private readonly IRequestClient<IGetPositionsRequest> _rcGetPositions;
    private readonly IRequestClient<IGetDepartmentsRequest> _rcGetDepartments;
    private readonly ILogger<GetCompanyCommand> _logger;

    private async Task<List<PositionData>> GetPositionsThroughBrokerAsync(
      List<string> errors)
    {
      const string errorMessage = "Can not get positions info. Please try again later.";

      try
      {
        Response<IOperationResult<IGetPositionsResponse>> response = await _rcGetPositions
          .GetResponse<IOperationResult<IGetPositionsResponse>>(
            IGetPositionsRequest.CreateObj());

        if (response.Message.IsSuccess)
        {
          _logger.LogInformation("Positions were taken from the service.");

          return response.Message.Body.Positions;
        }
        else
        {
          _logger.LogWarning("Errors while getting positions info. Reason: {Errors}",
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

    private async Task<List<DepartmentData>> GetDepartmensThroughBrokerAsync(
      List<string> errors)
    {
      const string errorMessage = "Can not get departments info. Please try again later.";

      try
      {
        Response<IOperationResult<IGetDepartmentsResponse>> response = await _rcGetDepartments
          .GetResponse<IOperationResult<IGetDepartmentsResponse>>(
            IGetDepartmentsRequest.CreateObj());

        if (response.Message.IsSuccess)
        {
          _logger.LogInformation("Departments were taken from the service.");

          return response.Message.Body.Departments;
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
      IRequestClient<IGetPositionsRequest> rcGetPositions,
      IRequestClient<IGetDepartmentsRequest> rcGetDepartments,
      ILogger<GetCompanyCommand> logger)
    {
      _repository = repository;
      _companyInfoMapper = mapper;
      _rcGetPositions = rcGetPositions;
      _rcGetDepartments = rcGetDepartments;
      _logger = logger;
    }

    public async Task<OperationResultResponse<CompanyInfo>> ExecuteAsync(GetCompanyFilter filter)
    {
      List<string> errors = new();
      DbCompany company = await _repository.GetAsync(filter);

      List<DepartmentData> departments = null;
      List<PositionData> positions = null;

      if (filter.IncludeDepartments)
      {
        departments = await GetDepartmensThroughBrokerAsync(errors);
      }

      if (filter.IncludePositions)
      {
        positions = await GetPositionsThroughBrokerAsync(errors);
      }

      return new OperationResultResponse<CompanyInfo>
      {
        Status = errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess,
        Body = _companyInfoMapper.Map(company, departments, positions, filter)
      };
    }
  }
}
