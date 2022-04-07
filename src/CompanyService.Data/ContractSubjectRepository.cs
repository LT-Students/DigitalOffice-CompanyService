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

    public async Task<Guid?> CreateAsync(DbContractSubject contractSubject)
    {
      if (contractSubject is null)
      {
        return null;
      }

      _provider.ContractSubjects.Add(contractSubject);
      await _provider.SaveAsync();

      return contractSubject.Id;
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

      IQueryable<DbContractSubject> contractSubjects = _provider.ContractSubjects.AsQueryable();

      if (filter.IsActive.HasValue)
      {
        contractSubjects = filter.IsActive.Value
          ? contractSubjects.Where(x => x.IsActive)
          : contractSubjects.Where(x => !x.IsActive);
      }

      return (
        await contractSubjects.Skip(filter.SkipCount).Take(filter.TakeCount).ToListAsync(),
        await contractSubjects.CountAsync());
    }

    public async Task<DbContractSubject> GetAsync(Guid contractSubjectId)
    {
      return await _provider.ContractSubjects.FirstOrDefaultAsync(x => x.Id == contractSubjectId);
    }

    public async Task<bool> DoesExistAsync(Guid contractSubjectId)
    {
      return await _provider.ContractSubjects.AnyAsync(x => x.Id == contractSubjectId);
    }

    public async Task<bool> DoesNameExistAsync(string name)
    {
      return await _provider.ContractSubjects.AnyAsync(x => string.Equals(x.Name.ToLower(), name.ToLower()));
    }
  }
}
