using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces
{
  [AutoInject]
    public interface IGetDepartmentCommand
    {
        Task<OperationResultResponse<DepartmentResponse>> Execute(GetDepartmentFilter filter);
    }
}
