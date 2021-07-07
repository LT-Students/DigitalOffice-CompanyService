using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using System;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
    [AutoInject]
    public interface IOfficeUserRepository
    {
        void Add(DbOfficeUser user);

        void ChangeOffice(Guid userId, Guid officeId, Guid changedBy);
    }
}
