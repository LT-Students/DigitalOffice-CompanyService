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
    /// <inheritdoc cref="IPositionRepository"/>
    public class PositionRepository : IPositionRepository
    {
        private readonly IDataProvider _provider;

        public PositionRepository(IDataProvider provider)
        {
            _provider = provider;
        }

        public DbPosition GetPosition(Guid? positionId, Guid? userId)
        {
            if (positionId.HasValue)
            {
                return _provider.Positions.FirstOrDefault(d => d.Id == positionId.Value)
                    ?? throw new NotFoundException($"There is not position with id {positionId}");
            }

            if (userId.HasValue)
            {
                return _provider.Positions
                    .Include(d => d.Users.Where(du => du.UserId == userId.Value))
                    .FirstOrDefault()
                    ?? throw new NotFoundException($"there is not position on which the user with id {userId} works");
            }

            throw new BadRequestException("You must specify 'positionId' or 'userId'.");
        }

        public List<DbPosition> FindPositions()
        {
            return _provider.Positions.ToList();
        }

        public DbPosition GetUserPosition(Guid userId)
        {
            var dbCompanyUser = _provider.PositionUsers
                .Include(u => u.Position)
                .FirstOrDefault(pu => pu.UserId == userId);

            if (dbCompanyUser == null)
            {
                throw new NotFoundException($"User with id: '{userId}' was not found.");
            }

            return _provider.Positions.Find(dbCompanyUser.PositionId);
        }

        public void DisablePosition(Guid positionId)
        {
            var dbPosition = _provider.Positions.FirstOrDefault(position => position.Id == positionId);

            if (dbPosition == null)
            {
                throw new NotFoundException($"Position with this id: '{positionId}' was not found.");
            }

            dbPosition.IsActive = false;
            _provider.Positions.Update(dbPosition);
            _provider.Save();
        }

        public Guid CreatePosition(DbPosition newPosition)
        {
            _provider.Positions.Add(newPosition);
            _provider.Save();

            return newPosition.Id;
        }

        public bool EditPosition(DbPosition newPosition)
        {
            var dbPosition = _provider.Positions.FirstOrDefault(position => position.Id == newPosition.Id);

            if (dbPosition == null)
            {
                throw new NotFoundException($"Position with this id: '{newPosition.Id}' was not found.");
            }

            dbPosition.Name = newPosition.Name;
            dbPosition.Description = newPosition.Description;
            _provider.Positions.Update(dbPosition);
            _provider.Save();

            return true;
        }
    }
}