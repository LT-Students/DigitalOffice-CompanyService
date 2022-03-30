using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Business.Helper;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Company.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Requests.Email;
using LT.DigitalOffice.Models.Broker.Requests.User;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company
{
  public class CreateCompanyCommand : ICreateCompanyCommand
  {
    private readonly IDbCompanyMapper _mapper;
    private readonly ILogger<ICreateCompanyCommand> _logger;
    private readonly ICreateCompanyRequestValidator _validator;
    private readonly ICompanyRepository _repository;
    private readonly IRequestClient<ICreateAdminRequest> _rcCreateAdmin;
    private readonly IRequestClient<ICreateSmtpCredentialsRequest> _rcCreateSmtp;
    private readonly ICompanyChangesRepository _companyChangesRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateCompanyCommand(
      IDbCompanyMapper mapper,
      ILogger<ICreateCompanyCommand> logger,
      ICreateCompanyRequestValidator validator,
      ICompanyRepository repository,
      IRequestClient<ICreateAdminRequest> rcCreateAdmin,
      IRequestClient<ICreateSmtpCredentialsRequest> rcCreateSmtp,
      ICompanyChangesRepository companyChangesRepository,
      IHttpContextAccessor httpContextAccessor)
    {
      _mapper = mapper;
      _logger = logger;
      _validator = validator;
      _repository = repository;
      _rcCreateAdmin = rcCreateAdmin;
      _rcCreateSmtp = rcCreateSmtp;
      _companyChangesRepository = companyChangesRepository;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<OperationResultResponse<Guid>> ExecuteAsync(CreateCompanyRequest request)
    {
      if (await _repository.GetAsync() != null)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return new OperationResultResponse<Guid>
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { "Company already exists" }
        };
      }

      if (!_validator.ValidateCustom(request, out List<string> errors))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return new OperationResultResponse<Guid>
        {
          Status = OperationResultStatusType.Failed,
          Errors = errors
        };
      }

      DbCompany company = _mapper.Map(request);

      await _repository.CreateAsync(company);

      //TODO async
      //Task.Run(() =>
      //{
      await _companyChangesRepository.CreateAsync(
        company.Id,
        null,
        CreateHistoryMessageHelper.Create(company));
      //}

      _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

      return new OperationResultResponse<Guid>
      {
        Status = OperationResultStatusType.FullSuccess,
        Body = company.Id
      };
    }
  }
}
