using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.CompanyService.Data
{
  public class CompanyRepository : ICompanyRepository
  {
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;

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

    public async Task<DbCompany> GetAsync(Guid companyId)
    {
      return await _provider.Companies.FirstOrDefaultAsync(x => x.Id == companyId);
    }

    public async Task<List<DbCompany>> GetAsync(IGetCompaniesRequest request)
    {
      IQueryable<DbCompany> dbCompanies = _provider.Companies.AsQueryable();

      if (request.UsersIds is not null && request.UsersIds.Any())
      {
        dbCompanies = dbCompanies.Where(d => d.IsActive && d.Users.Any(du => request.UsersIds.Contains(du.UserId)));
      }

      dbCompanies = dbCompanies
        .Include(d => d.Users.Where(du => du.IsActive))
        .ThenInclude(u => u.ContractSubject);

      return await dbCompanies.ToListAsync();
    }

    public async Task EditAsync(Guid companyId, JsonPatchDocument<DbCompany> request)
    {
      if (request == null)
      {
        return;
      }

      DbCompany company = await _provider.Companies.FirstOrDefaultAsync(x => x.Id == companyId);

      if (company == null)
      {
        return;
      }

      request.ApplyTo(company);
      company.ModifiedAtUtc = DateTime.UtcNow;
      company.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();

      await _provider.SaveAsync();
    }

    public async Task<bool> DoesExistAsync(Guid companyId)
    {
      return await _provider.Companies.AnyAsync(x => x.Id == companyId);
    }

    public async Task<bool> DoesExistAsync()
    {
      return await _provider.Companies.AnyAsync(x => x.IsActive);
    }
  }
}
