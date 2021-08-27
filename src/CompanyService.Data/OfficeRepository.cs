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
        private readonly IDataProvider _provider;

        public OfficeRepository(IDataProvider provider)
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

        public bool Contains(Guid officeId)
        {
            return _provider.Offices.Any(o => o.Id == officeId);
        }

        public List<DbOffice> Find(int skipCount, int takeCount, bool? includeDeactivated, out int totalCount)
        {
            if (skipCount < 0)
            {
                throw new ArgumentException("Skip count can't be less than 0.");
            }

            if (takeCount < 1)
            {
                throw new ArgumentException("Take count can't less than 1.");
            }

            IQueryable<DbOffice> dbOffices = _provider.Offices
                .AsQueryable();

            if (!(includeDeactivated.HasValue && includeDeactivated.Value))
            {
                dbOffices = dbOffices.Where(o => o.IsActive);
                totalCount = _provider.Offices.Count(o => o.IsActive);
            }
            else
            {
                totalCount = _provider.Offices.Count();
            }

            dbOffices = dbOffices.Skip(skipCount).Take(takeCount);

            return dbOffices.ToList();
        }

        public DbOffice Get(Guid officeId)
        {
            return _provider.Offices.FirstOrDefault(x => x.Id == officeId);
        }
    }
}
