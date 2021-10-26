using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.CompanyService.Data
{
  public class OfficeUserRepository : IOfficeUserRepository
  {
    private readonly IDataProvider _provider;

    public OfficeUserRepository(IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task<bool> CreateAsync(DbOfficeUser user)
    {
      if (user == null)
      {
        return false;
      }

      _provider.OfficeUsers.Add(user);
      await _provider.SaveAsync();

      return true;
    }

    public async Task<List<DbOfficeUser>> GetAsync(List<Guid> userIds)
    {
      return await _provider.OfficeUsers.Where(x => userIds.Contains(x.UserId) && x.IsActive).Include(x => x.Office).ToListAsync();
    }

    public async Task<Guid?> RemoveAsync(Guid userId, Guid removedBy)
    {
      DbOfficeUser user = await _provider.OfficeUsers.FirstOrDefaultAsync(u => u.UserId == userId && u.IsActive);

      if (user != null)
      {
        user.IsActive = false;
        user.ModifiedAtUtc = DateTime.UtcNow;
        user.ModifiedBy = removedBy;
        await _provider.SaveAsync();

        return user.OfficeId;
      }

      return null;
    }
  }
}
