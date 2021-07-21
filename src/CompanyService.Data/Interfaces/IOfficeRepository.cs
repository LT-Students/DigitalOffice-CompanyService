using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
    [AutoInject]
    public interface IOfficeRepository
    {
        void Add(DbOffice office);

        DbOffice Get(Guid officeId);

        List<DbOffice> Find(int skipCount, int takeCount, bool? includeDeactivated, out int totalCount);

        bool Contains(Guid officeId);
    }
}
