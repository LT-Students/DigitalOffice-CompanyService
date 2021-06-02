using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using System;

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
    }
}
