using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
    [AutoInject]
    public interface IPositionUserRepository
    {
        bool Add(DbPositionUser positionUser);

        DbPositionUser Get(Guid userId);

        List<DbPositionUser> Get(List<Guid> userIds);

        void Remove(Guid userId, Guid removedBy);
    }
}
