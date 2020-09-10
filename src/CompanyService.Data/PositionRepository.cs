using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Data
{
    public class PositionRepository : IPositionRepository
    {
        private readonly IDataProvider provider;

        public PositionRepository(IDataProvider provider)
        {
            this.provider = provider;
        }

        public DbPosition GetPositionById(Guid positionId)
        {
            var dbPosition = provider.Positions.FirstOrDefault(position => position.Id == positionId);

            if (dbPosition == null)
            {
                throw new Exception("Position with this id was not found.");
            }

            return dbPosition;
        }

        public List<DbPosition> GetPositionsList()
        {
            return provider.Positions.ToList();
        }

        public DbPosition GetUserPosition(Guid userId)
        {
            var dbCompanyUser = provider.CompaniesUsers
                .FirstOrDefault(companyUser => companyUser.UserId == userId);

            if (dbCompanyUser == null)
            {
                throw new Exception("Position not found.");
            }

            return provider.Positions.Find(dbCompanyUser.PositionId);
        }

        public void DisablePositionById(Guid positionId)
        {
            var dbPosition = provider.Positions.FirstOrDefault(position => position.Id == positionId);

            if (dbPosition == null)
            {
                throw new Exception("Position with this id was not found.");
            }

            dbPosition.IsActive = false;
            provider.Positions.Update(dbPosition);
            provider.Save();
        }

        public Guid AddPosition(DbPosition newPosition)
        {
            provider.Positions.Add(newPosition);
            provider.Save();

            return newPosition.Id;
        }

        public bool EditPosition(DbPosition newPosition)
        {
            var dbPosition = provider.Positions.FirstOrDefault(position => position.Id == newPosition.Id);

            if (dbPosition == null)
            {
                throw new Exception("Position with this id was not found.");
            }

            dbPosition.Name = newPosition.Name;
            dbPosition.Description = newPosition.Description;
            provider.Positions.Update(dbPosition);
            provider.Save();

            return true;
        }
    }
}