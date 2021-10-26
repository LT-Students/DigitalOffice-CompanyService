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

    public bool Add(DbOfficeUser user)
    {
      if (user == null)
      {
        throw new ArgumentNullException(nameof(user));
      }

      _provider.OfficeUsers.Add(user);
      _provider.Save();

      return true;
    }

    public List<DbOfficeUser> Get(List<Guid> userIds)
    {
      return _provider.OfficeUsers.Where(x => userIds.Contains(x.UserId) && x.IsActive).Include(x => x.Office).ToList();
    }

    public async Task<Guid?> RemoveAsync(Guid userId, Guid removedBy)
    {
      DbOfficeUser user = _provider.OfficeUsers.FirstOrDefault(u => u.UserId == userId && u.IsActive);

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
