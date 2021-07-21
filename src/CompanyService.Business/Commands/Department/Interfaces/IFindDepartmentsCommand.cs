using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces
{
    [AutoInject]
    public interface IFindDepartmentsCommand
    {
        FindResultResponse<DepartmentInfo> Execute(int skipCount, int takeCount, bool includeDeactivated);
    }
}
