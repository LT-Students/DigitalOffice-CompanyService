using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Business.Commands.ContractSubject.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject.Filters;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Validators.Interfaces;

namespace LT.DigitalOffice.CompanyService.Business.Commands.ContractSubject
{
  public class FindContractSubjectsCommand : IFindContractSubjectsCommand
  {
    private readonly IContractSubjectRepository _contractSubjectRepository;
    private readonly IContractSubjectInfoMapper _contractSubjectInfoMapper;
    private readonly IBaseFindFilterValidator _baseFindFilterValidator;
    private readonly IResponseCreator _responseCreator;

    public FindContractSubjectsCommand(
      IContractSubjectRepository contractSubjectRepository,
      IContractSubjectInfoMapper contractSubjectInfoMapper,
      IBaseFindFilterValidator baseFindFilterValidator,
      IResponseCreator responseCreator)
    {
      _contractSubjectRepository = contractSubjectRepository;
      _contractSubjectInfoMapper = contractSubjectInfoMapper;
      _baseFindFilterValidator = baseFindFilterValidator;
      _responseCreator = responseCreator;
    }

    public async Task<FindResultResponse<ContractSubjectInfo>> ExecuteAsync(FindContractSubjectFilter filter)
    {
      if (!_baseFindFilterValidator.ValidateCustom(filter, out List<string> errors))
      {
        return _responseCreator.CreateFailureFindResponse<ContractSubjectInfo>(HttpStatusCode.BadRequest, errors);
      }

      (List<DbContractSubject> dbContractSubjects, int totalCount) = await _contractSubjectRepository.FindAsync(filter);

      FindResultResponse<ContractSubjectInfo> response = new();

      response.Body = dbContractSubjects.Select(cs => _contractSubjectInfoMapper.Map(cs)).ToList();
      response.TotalCount = totalCount;

      return response;
    }
  }
}
