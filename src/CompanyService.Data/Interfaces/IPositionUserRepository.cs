using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using System;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
    [AutoInject]
    public interface IPositionUserRepository
    {
        public bool Add(DbPositionUser positionUser);
        public DbPositionUser Get(Guid userId, bool includePosition);
        public void Remove(Guid userId);
    }
}
