using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Business.Commands.ContractSubject.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject.Filters;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.CompanyService.Business.Commands.ContractSubject
{
  public class FindContractSubjectsCommand : IFindContractSubjectsCommand
  {
    private readonly IContractSubjectRepository _contractSubjectRepository;
    private readonly IContractSubjectInfoMapper _contractSubjectInfoMapper;

    public FindContractSubjectsCommand(
      IContractSubjectRepository contractSubjectRepository,
      IContractSubjectInfoMapper contractSubjectInfoMapper)
    {
      _contractSubjectRepository = contractSubjectRepository;
      _contractSubjectInfoMapper = contractSubjectInfoMapper;
    }

    public async Task<FindResultResponse<ContractSubjectInfo>> ExecuteAsync(FindContractSubjectFilter filter)
    {
      (List<DbContractSubject> dbContractSubjects, int totalCount) = await _contractSubjectRepository.FindAsync(filter);

      return new FindResultResponse<ContractSubjectInfo>(
        body: dbContractSubjects.Select(cs => _contractSubjectInfoMapper.Map(cs)).ToList(),
        totalCount: totalCount);
    }
  }
}
