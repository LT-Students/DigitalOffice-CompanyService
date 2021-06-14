using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using System;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
    [AutoInject]
    public interface IPositionUserRepository
    {
        bool Add(DbPositionUser positionUser);

        DbPositionUser Get(Guid userId, bool includePosition);

        void Remove(Guid userId);
    }
}
