using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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
            return _provider.OfficeUsers.Where(x => userIds.Contains(x.UserId)).Include(x => x.Office).ToList();
        }

        public void Remove(Guid userId, Guid removedBy)
        {
            DbOfficeUser user = _provider.OfficeUsers.FirstOrDefault(u => u.UserId == userId && u.IsActive);

            if (user != null)
            {
                user.IsActive = false;
                user.ModifiedAtUtc = DateTime.UtcNow;
                user.ModifiedBy = removedBy;
                _provider.Save();
            }
        }
    }
}
