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

        DbPositionUser Get(Guid userId, bool includePosition);

        List<DbPositionUser> Find(List<Guid> userIds);

        void CheckAndRemovePositionUser(Guid userId);
    }
}
