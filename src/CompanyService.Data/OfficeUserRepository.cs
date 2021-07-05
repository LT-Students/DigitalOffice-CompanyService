using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using System;
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

        public DbOfficeUser Find(Guid userId)
        {
            return _provider.OfficeUsers.FirstOrDefault(x => x.UserId == userId)
                ?? throw new NotFoundException($"No OfficeUser with user with id '{userId}'");
        }

        public DbOfficeUser Get(Guid id)
        {
            return _provider.OfficeUsers.FirstOrDefault(x => x.Id == id)
                ?? throw new NotFoundException($"No OfficeUser with id '{id}'");
        }

        public void Remove(Guid userId, Guid removedBy)
        {
            var user = _provider.OfficeUsers.FirstOrDefault(x => x.UserId == userId);

            if (user == null)
            {
                return;
            }

            user.RemovedBy = removedBy;
            user.RemovedAt = DateTime.UtcNow;

            _provider.Save();
        }
    }
}
