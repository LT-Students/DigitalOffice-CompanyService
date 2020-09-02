using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.CompanyService.Database;
using LT.DigitalOffice.CompanyService.Database.Entities;
using LT.DigitalOffice.CompanyService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.CompanyService.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private readonly CompanyServiceDbContext dbContext;

        public PositionRepository([FromServices] CompanyServiceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public DbPosition GetPositionById(Guid positionId)
        {
            var dbPosition = dbContext.Positions.FirstOrDefault(position => position.Id == positionId);

            if (dbPosition == null)
            {
                throw new Exception("Position with this id was not found.");
            }

            return dbPosition;
        }

        public List<DbPosition> GetPositionsList()
        {
            return dbContext.Positions.ToList();
        }

        public DbPosition GetUserPosition(Guid userId)
        {
            var dbCompanyUser = dbContext.CompaniesUsers
                .FirstOrDefault(companyUser => companyUser.UserId == userId);

            if (dbCompanyUser == null)
            {
                throw new Exception("Position not found.");
            }

            return dbContext.Positions.Find(dbCompanyUser.PositionId);
        }

        public void DisablePositionById(Guid positionId)
        {
            var dbPosition = dbContext.Positions.FirstOrDefault(position => position.Id == positionId);

            if (dbPosition == null)
            {
                throw new Exception("Position with this id was not found.");
            }

            dbPosition.IsActive = false;
            dbContext.Positions.Update(dbPosition);
            dbContext.SaveChanges();
        }

        public Guid AddPosition(DbPosition newPosition)
        {
            dbContext.Positions.Add(newPosition);
            dbContext.SaveChanges();

            return newPosition.Id;
        }

        public bool EditPosition(DbPosition newPosition)
        {
            var dbPosition = dbContext.Positions.FirstOrDefault(position => position.Id == newPosition.Id);

            if (dbPosition == null)
            {
                throw new Exception("Position with this id was not found.");
            }

            dbPosition.Name = newPosition.Name;
            dbPosition.Description = newPosition.Description;
            dbContext.Positions.Update(dbPosition);
            dbContext.SaveChanges();

            return true;
        }
    }
}