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
    public class PositionUserRepository : IPositionUserRepository
    {
        private readonly IDataProvider _provider;

        public PositionUserRepository(
            IDataProvider provider)
        {
            _provider = provider;
        }

        public bool Add(DbPositionUser positionUser)
        {
            if (positionUser == null)
            {
                throw new ArgumentNullException(nameof(positionUser));
            }

            _provider.PositionUsers.Add(positionUser);
            _provider.Save();

            return true;
        }

        public DbPositionUser Get(Guid userId)
        {
            return _provider.PositionUsers.Include(u => u.Position).FirstOrDefault(u => u.UserId == userId && u.IsActive);
        }

        public List<DbPositionUser> Get(List<Guid> userIds)
        {
            return _provider.PositionUsers
                .Include(pu => pu.Position)
                .Where(u => userIds.Contains(u.UserId) && u.IsActive)
                .ToList();
        }

        public void Remove(Guid userId, Guid removedBy)
        {
            DbPositionUser user = _provider.PositionUsers.FirstOrDefault(u => u.UserId == userId && u.IsActive);

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
