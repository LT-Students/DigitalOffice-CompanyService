using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject.Filters;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
  public class ContractSubjectRepository : IContractSubjectRepository
  {
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ContractSubjectRepository(
      IDataProvider provider,
      IHttpContextAccessor httpContextAccessor)
    {
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
    }

    public Task CreateAsync(DbContractSubject contractSubject)
    {
      if (contractSubject is null)
      {
        return null;
      }

      _provider.ContractSubjects.Add(contractSubject);
      return _provider.SaveAsync();
    }

    public async Task<bool> EditAsync(Guid contractSubjectId, JsonPatchDocument<DbContractSubject> request)
    {
      if (request is null)
      {
        return false;
      }

      DbContractSubject dbContractSubject = await GetAsync(contractSubjectId);

      if (dbContractSubject is null)
      {
        return false;
      }

      request.ApplyTo(dbContractSubject);
      dbContractSubject.ModifiedAtUtc = DateTime.UtcNow;
      dbContractSubject.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();

      await _provider.SaveAsync();

      return true;
    }

    public async Task<(List<DbContractSubject> dbContractSubjects, int totalCount)> FindAsync(FindContractSubjectFilter filter)
    {
      if (filter is null)
      {
        return default;
      }

      IQueryable<DbContractSubject> contractSubjectQuery = _provider.ContractSubjects.AsQueryable();

      if (filter.IsActive.HasValue)
      {
        contractSubjectQuery = filter.IsActive.Value
          ? contractSubjectQuery.Where(x => x.IsActive)
          : contractSubjectQuery.Where(x => !x.IsActive);
      }

      return (
        await contractSubjectQuery.Skip(filter.SkipCount).Take(filter.TakeCount).ToListAsync(),
        await contractSubjectQuery.CountAsync());
    }

    public Task<DbContractSubject> GetAsync(Guid contractSubjectId)
    {
      return _provider.ContractSubjects.FirstOrDefaultAsync(x => x.Id == contractSubjectId);
    }

    public Task<bool> DoesExistAsync(Guid contractSubjectId)
    {
      return _provider.ContractSubjects.AnyAsync(x => x.Id == contractSubjectId);
    }

    public Task<bool> DoesNameExistAsync(string name)
    {
      return _provider.ContractSubjects.AnyAsync(x => string.Equals(x.Name.ToLower(), name.ToLower()));
    }
  }
}
