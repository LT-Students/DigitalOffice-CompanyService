using System;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Business.Commands.ContractSubject.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.CompanyService.Business.Commands.ContractSubject
{
  public class CreateContractSubjectCommand : ICreateContractSubjectCommand
  {
    private readonly IContractSubjectRepository _contractSubjectRepository;
    private readonly IAccessValidator _accessValidator;
    private readonly IResponseCreator _responseCreator;
    private readonly IDbContractSubjectMapper _dbContractSubjectMapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateContractSubjectCommand(
      IContractSubjectRepository contractSubjectRepository,
      IAccessValidator accessValidator,
      IResponseCreator responseCreator,
      IDbContractSubjectMapper dbContractSubjectMapper,
      IHttpContextAccessor httpContextAccessor)
    {
      _contractSubjectRepository = contractSubjectRepository;
      _accessValidator = accessValidator;
      _responseCreator = responseCreator;
      _dbContractSubjectMapper = dbContractSubjectMapper;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateContractSubjectRequest request)
    {
      //which rights?
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveCompanies)
        || !await _accessValidator.HasRightsAsync(Rights.AddEditRemoveCompanyData))
      {
        return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.Forbidden);
      }

      //validate

      OperationResultResponse<Guid?> response = new();

      response.Body = await _contractSubjectRepository.CreateAsync(
        _dbContractSubjectMapper.Map(request));

      if (response.Body is null)
      {
        return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.BadRequest);
      }

      _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

      return response;
    }
  }
}
