using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
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

        public List<DbOffice> Find(int skipCount, int takeCount)
        {
            return _provider.Offices.Skip(skipCount * takeCount).Take(takeCount).Where(o => o.IsActive).ToList();
        }
    }
}
