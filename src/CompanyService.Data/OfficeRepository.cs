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

        public List<DbOffice> Find(int skipCount, int takeCount, bool? includeDeactivated, out int totalCount)
        {
            if (takeCount <= 0)
            {
                throw new BadRequestException("Take count can't be equal or less than 0.");
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
            return _provider.Offices.FirstOrDefault(x => x.Id == officeId)
                ?? throw new NotFoundException($"No office with id '{officeId}'");
        }
    }
}
