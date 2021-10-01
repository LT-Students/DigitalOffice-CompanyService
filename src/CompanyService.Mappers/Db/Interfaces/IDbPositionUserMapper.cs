using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces
{
  [AutoInject]
    public interface IDbPositionUserMapper
    {
        DbPositionUser Map(Guid userId, Guid positionId, Guid modifiedBy);
    }
}
