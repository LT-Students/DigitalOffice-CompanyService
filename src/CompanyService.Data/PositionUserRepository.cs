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

            if (_provider.Positions.Any(p => p.Id == positionUser.PositionId && p.IsActive == false))
            {
                throw new BadRequestException($"Position id: {positionUser.PositionId} is not active");
            }

            _provider.PositionUsers.Add(positionUser);
            _provider.Save();

            return true;
        }

        public DbPositionUser Get(Guid userId, bool includePosition)
        {
            DbPositionUser user = null;

            if (includePosition)
            {
                user = _provider.PositionUsers.Include(u => u.Position).FirstOrDefault(u => u.UserId == userId && u.IsActive == true);
            }
            else
            {
                user = _provider.PositionUsers.FirstOrDefault(u => u.UserId == userId && u.IsActive == true);
            }

            if (user == null)
            {
                throw new NotFoundException($"There is not user in position with id {userId}");
            }

            return user;
        }

        public List<DbPositionUser> Find(List<Guid> userIds)
        {
            return _provider.PositionUsers
                .Include(pu => pu.Position)
                .Where(u => userIds.Contains(u.UserId) && u.IsActive == true)
                .ToList();
        }

        public void Remove(Guid userId)
        {
            DbPositionUser user = _provider.PositionUsers.FirstOrDefault(u => u.UserId == userId && u.IsActive);

            if (user != null)
            {
                user.IsActive = false;
                _provider.Save();
            }
            else
            {
                throw new NotFoundException($"There is not user in position with id {userId}");
            }
        }
    }
}
