using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.CompanyService.Data
{
  public class CompanyUserRepository : ICompanyUserRepository
  {
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CompanyUserRepository(
      IDataProvider provider,
      IHttpContextAccessor httpContextAccessor)
    {
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Guid?> CreateAsync(DbCompanyUser dbCompanyUser)
    {
      if (dbCompanyUser is null)
      {
        return null;
      }

      _provider.CompaniesUsers.Add(dbCompanyUser);
      await _provider.SaveAsync();

      return dbCompanyUser.Id;
    }

    public async Task<bool> EditAsync(Guid userId, JsonPatchDocument<DbCompanyUser> request)
    {
      DbCompanyUser dbCompanyUser = await _provider.CompaniesUsers.FirstOrDefaultAsync(x => x.UserId == userId);

      if (request is null || dbCompanyUser is null)
      {
        return false;
      }

      request.ApplyTo(dbCompanyUser);
      dbCompanyUser.ModifiedAtUtc = DateTime.UtcNow;
      dbCompanyUser.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      await _provider.SaveAsync();

      return true;
    }

    public async Task<DbCompanyUser> GetAsync(Guid userId)
    {
      return await _provider.CompaniesUsers.Include(u => u.Company).FirstOrDefaultAsync(u => u.UserId == userId && u.IsActive);
    }

    public async Task<List<DbCompanyUser>> GetAsync(List<Guid> userIds)
    {
      return await _provider.CompaniesUsers
        .Include(cu => cu.Company)
        .Where(u => userIds.Contains(u.UserId) && u.IsActive)
        .ToListAsync();
    }

    public async Task RemoveAsync(Guid userId, Guid removedBy)
    {
      DbCompanyUser user = await _provider.CompaniesUsers.FirstOrDefaultAsync(u => u.UserId == userId && u.IsActive);

      if (user != null)
      {
        user.IsActive = false;
        user.ModifiedAtUtc = DateTime.UtcNow;
        user.ModifiedBy = removedBy;
        await _provider.SaveAsync();
      }
    }
  }
}

