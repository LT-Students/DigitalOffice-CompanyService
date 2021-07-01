using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
    [AutoInject]
    public interface IOfficeRepository
    {
        void Add(DbOffice office);

        List<DbOffice> Find(int skipCount, int takeCount);
    }
}
