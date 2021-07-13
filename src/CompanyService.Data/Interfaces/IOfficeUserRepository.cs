using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
    [AutoInject]
    public interface IOfficeUserRepository
    {
        void Add(DbOfficeUser user);

        void ChangeOffice(Guid userId, Guid officeId, Guid changedBy);

        List<DbOfficeUser> Get(List<Guid> userIds);
    }
}
