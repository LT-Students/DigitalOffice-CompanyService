using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces
{
    [AutoInject]
    public interface IFindDepartmentsCommand
    {
        FindDepartmentsResponse Execute(int skipCount, int takeCount, bool includeDeactivated);
    }
}
