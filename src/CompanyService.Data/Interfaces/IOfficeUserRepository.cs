using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using System;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
    [AutoInject]
    public interface IOfficeUserRepository
    {
        void Add(DbOfficeUser user);
        DbOfficeUser Get(Guid id);
        DbOfficeUser Find(Guid userId);
        void Remove(Guid id, Guid removedBy);
    }
}
