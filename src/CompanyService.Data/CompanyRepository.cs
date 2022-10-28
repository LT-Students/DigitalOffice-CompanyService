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
      if (company is null)
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

    public Task<DbCompany> GetAsync()
    {
      return _provider.Companies.FirstOrDefaultAsync();
    }

    public Task<List<DbCompany>> GetAsync(IGetCompaniesRequest request)
    {
      IQueryable<DbCompany> query = _provider.Companies.AsQueryable();

      if (request.UsersIds is not null && request.UsersIds.Any())
      {
        query = query.Where(d => d.IsActive && d.Users.Any(du => request.UsersIds.Contains(du.UserId)))
          .Include(d => d.Users.Where(du => du.IsActive && request.UsersIds.Contains(du.UserId)))
          .ThenInclude(u => u.ContractSubject);
      }

      return query.ToListAsync();
    }

    public async Task EditAsync(Guid companyId, JsonPatchDocument<DbCompany> request)
    {
      if (request is null)
      {
        return;
      }

      DbCompany company = await _provider.Companies.FirstOrDefaultAsync(x => x.Id == companyId);

      if (company is null)
      {
        return;
      }

      request.ApplyTo(company);
      company.ModifiedAtUtc = DateTime.UtcNow;
      company.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();

      await _provider.SaveAsync();
    }

    public Task<bool> DoesExistAsync(Guid companyId)
    {
      return _provider.Companies.AnyAsync(x => x.Id == companyId);
    }

    public Task<bool> DoesExistAsync()
    {
      return _provider.Companies.AnyAsync(x => x.IsActive);
    }

    public Task<bool> DoesNameExistAsync(string name)
    {
      return _provider.Companies.AnyAsync(x => string.Equals(x.Name.ToLower(), name.ToLower()));
    }
  }
}
