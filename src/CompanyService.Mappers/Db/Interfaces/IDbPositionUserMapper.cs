using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces
{
    [AutoInject]
    public interface IDbPositionUserMapper
    {
        DbPositionUser Map(IChangeUserPositionRequest request);
    }
}
