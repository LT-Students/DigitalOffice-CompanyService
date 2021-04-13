using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.UserService.Models.Broker.Models;

namespace LT.DigitalOffice.CompanyService.Mappers.RequestMappers.Interfaces
{
    [AutoInject]
    public interface IUserMapper : IMapper<UserData, User>
    {
    }
}
