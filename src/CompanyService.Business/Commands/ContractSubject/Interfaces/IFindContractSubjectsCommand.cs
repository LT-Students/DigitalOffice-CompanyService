using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject.Filters;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.CompanyService.Business.Commands.ContractSubject.Interfaces
{
  [AutoInject]
  public interface IFindContractSubjectsCommand
  {
    Task<FindResultResponse<ContractSubjectInfo>> ExecuteAsync(FindContractSubjectFilter filter);
  }
}
