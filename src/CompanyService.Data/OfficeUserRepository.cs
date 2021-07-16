using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
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

        public void Add(DbOfficeUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _provider.OfficeUsers.Add(user);
            _provider.Save();
        }

        public void ChangeOffice(Guid userId, Guid officeId, Guid changedBy)
        {
            var officeUser = _provider.OfficeUsers.FirstOrDefault(x => x.UserId == userId);

            if (officeUser != null)
            {
                officeUser.OfficeId = officeId;
                _provider.Save();

                return;
            }

            _provider.OfficeUsers.Add(
                new DbOfficeUser
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    OfficeId = officeId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = changedBy,
                    IsActive = true
                });
            _provider.Save();
        }

        public List<DbOfficeUser> Get(List<Guid> userIds)
        {
            return _provider.OfficeUsers.Where(x => userIds.Contains(x.UserId)).Include(x => x.Office).ToList();
        }
    }
}
