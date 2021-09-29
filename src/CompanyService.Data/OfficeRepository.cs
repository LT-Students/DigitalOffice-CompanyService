using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.CompanyService.Data
{
  public class OfficeRepository : IOfficeRepository
  {
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<OfficeRepository> _logger;

    public OfficeRepository(
      IDataProvider provider,
      IHttpContextAccessor httpContextAccessor,
      ILogger<OfficeRepository> logger)
    {
      _httpContextAccessor = httpContextAccessor;
      _provider = provider;
      _logger = logger;
    }

    public void Add(DbOffice office)
    {
      if (office == null)
      {
        _logger.LogWarning(new ArgumentNullException(nameof(office)).Message);
        return;
      }

      _provider.Offices.Add(office);
      _provider.Save();
    }

    public bool Contains(Guid officeId)
    {
      return _provider.Offices.Any(o => o.Id == officeId);
    }

    public List<DbOffice> Find(BaseFindFilter filter, out int totalCount)
    {
      if (filter == null)
      {
        totalCount = 0;
        return null;
      }

      IQueryable<DbOffice> dbOffices = _provider.Offices
        .AsQueryable();

      if (!filter.IncludeDeactivated)
      {
        dbOffices = dbOffices.Where(x => x.IsActive);
      }

      totalCount = dbOffices.Count();

      return dbOffices.Skip(filter.skipCount).Take(filter.takeCount).ToList();
    }

    public bool Edit(Guid officeId, JsonPatchDocument<DbOffice> request)
    {
      DbOffice dbOffice = _provider.Offices.FirstOrDefault(x => x.Id == officeId);

      if (dbOffice == null || request == null)
      {
        return false;
      }

      request.ApplyTo(dbOffice);
      dbOffice.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      dbOffice.ModifiedAtUtc = DateTime.UtcNow;
      _provider.Save();

      return true;
    }

    public DbOffice Get(Guid officeId)
    {
      return _provider.Offices.FirstOrDefault(x => x.Id == officeId);
    }
  }
}
