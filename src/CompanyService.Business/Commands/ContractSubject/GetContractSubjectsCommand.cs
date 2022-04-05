using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Business.Commands.ContractSubject.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.CompanyService.Business.Commands.ContractSubject
{
  public class GetContractSubjectsCommand : IGetContractSubjectsCommand
  {
    private readonly IContractSubjectRepository _contractSubjectRepository;
    private readonly IContractSubjectInfoMapper _contractSubjectInfoMapper;

    public GetContractSubjectsCommand(
      IContractSubjectRepository contractSubjectRepository,
      IContractSubjectInfoMapper contractSubjectInfoMapper)
    {
      _contractSubjectRepository = contractSubjectRepository;
      _contractSubjectInfoMapper = contractSubjectInfoMapper;
    }

    public async Task<OperationResultResponse<List<ContractSubjectInfo>>> ExecuteAsync(Guid companyId)
    {
      List<DbContractSubject> dbContractSubjects = await _contractSubjectRepository.FindAsync(companyId);

      OperationResultResponse<List<ContractSubjectInfo>> response = new();

      response.Body = dbContractSubjects.Select(cs => _contractSubjectInfoMapper.Map(cs)).ToList();

      return response;
    }
  }
}
