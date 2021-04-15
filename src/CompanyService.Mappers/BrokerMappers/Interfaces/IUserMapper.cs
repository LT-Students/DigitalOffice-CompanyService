using LT.DigitalOffice.Broker.Models;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Mappers.RequestMappers.Interfaces
{
    [AutoInject]
    public interface IUserMapper : IMapper<UserData, User>
    {
    }
}
