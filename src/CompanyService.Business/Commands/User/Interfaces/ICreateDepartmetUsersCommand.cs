using LT.DigitalOffice.CompanyService.Models.Dto.Requests.User;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.CompanyService.Business.Commands.User.Interfaces
{
  [AutoInject]
  public interface ICreateDepartmetUsersCommand
  {
    OperationResultResponse<bool> Execute(CreateDepartmentUsersRequest request);
  }
}
