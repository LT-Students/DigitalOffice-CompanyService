using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models.Department;

namespace LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces
{
  [AutoInject]
  public interface IDepartmentInfoMapper
  {
    DepartmentInfo Map(DepartmentData department);
  }
}
