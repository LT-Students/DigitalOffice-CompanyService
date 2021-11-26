using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Models.Department;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
  public class DepartmentInfoMapper : IDepartmentInfoMapper
  {
    public DepartmentInfo Map(DepartmentData department)
    {
      if (department == null)
      {
        return null;
      }

      return new DepartmentInfo
      {
        Id = department.Id,
        Name = department.Name
      };
    }
  }
}
