using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company
{
  public class GetCompanyCommand : IGetCompanyCommand
  {
    private readonly ICompanyRepository _repository;
    private readonly ICompanyInfoMapper _companyInfoMapper;

    public GetCompanyCommand(
      ICompanyRepository repository,
      ICompanyInfoMapper mapper,
      ILogger<GetCompanyCommand> logger,
      IImageInfoMapper imageMapper)
    {
      _repository = repository;
      _companyInfoMapper = mapper;
    }

    public OperationResultResponse<CompanyInfo> Execute(GetCompanyFilter filter)
    {
      List<string> errors = new();
      DbCompany company = _repository.Get(filter);

      return new OperationResultResponse<CompanyInfo>
      {
        Status = errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess,
        Body = _companyInfoMapper.Map(company, filter)
      };
    }
  }
}
