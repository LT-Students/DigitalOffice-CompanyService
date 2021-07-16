using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models;

namespace LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces
{
    [AutoInject]
    public interface IShortDepartmentInfoMapper
    {
        ShortDepartmentInfo Map(DbDepartment department, UserData userData, DbPositionUser dbPositionUser, ImageData image);
    }
}
