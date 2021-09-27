using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.User;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.CompanyService.Business.Commands.User.Interfaces
{
  [AutoInject]
  public interface IAddDepartmetUsersCommand
  {
    Task<OperationResultResponse<bool>> Execute(AddDepartmentUsersRequest request);
  }
}
