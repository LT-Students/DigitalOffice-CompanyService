using System;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.CompanyService.Data
{
  public class CompanyRepository : ICompanyRepository
  {
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private IQueryable<DbCompany> CreateGetPredicates(
      GetCompanyFilter filter,
      IQueryable<DbCompany> dbCompanies)
    {
      if (filter == null)
      {
        return dbCompanies;
      }

      if (filter.IncludeOffices)
      {
        dbCompanies = dbCompanies.Include(c => c.Offices.Where(o => o.IsActive));
      }

      return dbCompanies;
    }

    public CompanyRepository(
      IDataProvider provider,
      IHttpContextAccessor httpContextAccessor)
    {
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task CreateAsync(DbCompany company)
    {
      if (company == null)
      {
        return;
      }

      if (await _provider.Companies.AnyAsync())
      {
        return;
      }

      _provider.Companies.Add(company);
      await _provider.SaveAsync();
    }

    public async Task<DbCompany> GetAsync(GetCompanyFilter filter = null)
    {
      var dbUsers = _provider.Companies
        .AsSingleQuery()
        .AsQueryable();

      return await CreateGetPredicates(filter, dbUsers).FirstOrDefaultAsync();
    }

    public async Task EditAsync(JsonPatchDocument<DbCompany> request)
    {
      if (request == null)
      {
        return;
      }

      var company = await _provider.Companies.FirstOrDefaultAsync();

      if (company == null)
      {
        return;
      }

      request.ApplyTo(company);
      company.ModifiedAtUtc = DateTime.UtcNow;
      company.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();

      await _provider.SaveAsync();
    }
  }
}
