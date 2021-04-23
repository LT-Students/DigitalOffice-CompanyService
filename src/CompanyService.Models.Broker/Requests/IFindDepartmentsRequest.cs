using LT.DigitalOffice.CompanyService.Models.Dto.Configuration;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.Broker.Requests
{
    public interface IFindDepartmentsRequest
    {
        string DepartmentName { get; }

        static object CreateObj(string departmentName)
        {
            return new
            {
                DepartmentName = departmentName
            };
        }
    }
}
