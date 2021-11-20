using System;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;

namespace LT.DigitalOffice.CompanyService.Data
{
  public class CompanyChangesRepository : ICompanyChangesRepository
  {
    private IDataProvider _provider;

    public CompanyChangesRepository(IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task CreateAsync(Guid companyId, Guid? changedBy, string changes)
    {
      _provider.AddCompanyOrChanges(
        new DbCompanyChanges
        {
          Id = Guid.NewGuid(),
          CompanyId = companyId,
          ModifiedBy = changedBy,
          ModifiedAtUtc = DateTime.UtcNow,
          Changes = changes
        });

      await _provider.SaveAsync();
    }
  }
}
