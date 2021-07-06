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

        public void Add(Guid companyId, Guid? changedBy, string changes)
        {
            _provider.CompanyChanges.Add(
                new DbCompanyChanges
                {
                    Id = Guid.NewGuid(),
                    CompanyId = companyId,
                    UserId = changedBy,
                    ModifiedAt = DateTime.UtcNow,
                    Changes = changes
                });
            _provider.Save();
        }
    }
}
