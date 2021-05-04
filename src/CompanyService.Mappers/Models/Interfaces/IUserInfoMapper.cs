using LT.DigitalOffice.Broker.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces
{
    [AutoInject]
    public interface IUserInfoMapper
    {
        UserInfo Map(UserData userData);
    }
}
