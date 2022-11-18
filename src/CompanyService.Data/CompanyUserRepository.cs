using System;
using System.Linq;
using System.Threading.Tasks;
using DigitalOffice.Models.Broker.Publishing;
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

    public Task CreateAsync(DbCompanyUser dbCompanyUser)
    {
      if (dbCompanyUser is null)
      {
        return null;
      }

      _provider.CompaniesUsers.Add(dbCompanyUser);
      return _provider.SaveAsync();
    }

    public async Task<bool> EditAsync(Guid userId, JsonPatchDocument<DbCompanyUser> request)
    {
      DbCompanyUser dbCompanyUser = await _provider.CompaniesUsers.FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive);

      if (request is null || dbCompanyUser is null)
      {
        return false;
      }

      request.ApplyTo(dbCompanyUser);
      dbCompanyUser.CreatedBy = _httpContextAccessor.HttpContext.GetUserId();

      await _provider.SaveAsync();

      return true;
    }

    public Task<DbCompanyUser> GetAsync(Guid userId)
    {
      return _provider.CompaniesUsers
        .Include(u => u.Company)
        .FirstOrDefaultAsync(u => u.UserId == userId && u.IsActive);
    }

    public async Task<Guid?> ActivateAsync(IActivateUserPublish request)
    {
      DbCompanyUser user = await _provider.CompaniesUsers.FirstOrDefaultAsync(u => u.UserId == request.UserId && !u.IsActive);

      if (user is null)
      {
        return null;
      }

      user.IsActive = true;
      await _provider.SaveAsync();

      return user.CompanyId;
    }

    public async Task<Guid?> RemoveAsync(Guid userId, Guid removedBy)
    {
      DbCompanyUser user = await _provider.CompaniesUsers.FirstOrDefaultAsync(u => u.UserId == userId && u.IsActive);

      if (user is null)
      {
        return null;
      }

      user.IsActive = false;
      user.CreatedBy = removedBy;

      await _provider.SaveAsync();

      return user.CompanyId;
    }

    public Task<bool> DoesExistAsync(Guid userId)
    {
      return _provider.CompaniesUsers.AnyAsync(u => u.UserId == userId);
    }

    public async Task<bool> RemoveContractSubjectAsync(Guid contractSubjectId)
    {
      IQueryable<DbCompanyUser> dbUsers = _provider.CompaniesUsers.Where(x => x.ContractSubjectId == contractSubjectId);

      foreach (DbCompanyUser user in dbUsers)
      {
        user.ContractSubjectId = null;
      }

      await _provider.SaveAsync();

      return true;
    }
  }
}

