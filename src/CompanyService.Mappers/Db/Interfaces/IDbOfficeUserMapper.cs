using System;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces
{
  [AutoInject]
    public interface IDbOfficeUserMapper
    {
        DbOfficeUser Map(Guid userId, Guid officeId, Guid modifiedBy);
    }
}
