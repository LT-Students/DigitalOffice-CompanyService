using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using System;

namespace LT.DigitalOffice.CompanyService.Data
{
    public class CompanyChangesRepository : ICompanyChangesRepository
    {
        private IDataProvider _provider;

        public CompanyChangesRepository(IDataProvider provider)
        {
            _provider = provider;
        }

        public void Add(DbCompanyChanges dbCompanyChanges)
        {
            if (dbCompanyChanges == null)
            {
                throw new ArgumentNullException(nameof(dbCompanyChanges));
            }

            _provider.CompanyChanges.Add(dbCompanyChanges);
            _provider.Save();
        }
    }
}
