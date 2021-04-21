using LT.DigitalOffice.CompanyService.Models.Dto.Configuration;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.Broker.Requests
{
    [AutoInjectRequest(nameof(RabbitMqConfig.FindDepartmentEndpoint))]
    public interface IFindDepartmentsRequest
    {
        string DepartmentName { get; set; }

        static object CreateObj(string departmentName)
        {
            return new
            {
                DepartmentName = departmentName
            };
        }
    }
}
