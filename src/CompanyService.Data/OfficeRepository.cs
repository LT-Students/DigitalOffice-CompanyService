using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Data
{
    public class OfficeRepository : IOfficeRepository
    {
        private IDataProvider _provider;

        public OfficeRepository(
            IDataProvider provider)
        {
            _provider = provider;
        }

        public void Add(DbOffice office)
        {
            if (office == null)
            {
                throw new ArgumentNullException(nameof(office));
            }

            _provider.Offices.Add(office);
            _provider.Save();
        }

        public List<DbOffice> Find(int skipCount, int takeCount, out int totalCount)
        {
            if (takeCount <= 0)
            {
                throw new BadRequestException("Take count can't be equal or less than 0.");
            }

            totalCount = _provider.Offices.Count(x => x.IsActive);

            return _provider.Offices.Skip(skipCount * takeCount).Take(takeCount).Where(o => o.IsActive).ToList();
        }
    }
}
