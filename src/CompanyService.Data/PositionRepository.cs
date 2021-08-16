﻿using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PositionRepository(
            IDataProvider provider,
            IHttpContextAccessor httpContextAccessor)
        {
            _provider = provider;
            _httpContextAccessor = httpContextAccessor;
        }

        public DbPosition Get(Guid? positionId, Guid? userId)
        {
            if (positionId.HasValue)
            {
                return _provider.Positions.FirstOrDefault(d => d.Id == positionId.Value)
                    ?? throw new NotFoundException($"There is not position with id {positionId}");
            }

            if (userId.HasValue)
            {
                return _provider.PositionUsers
                    .Include(pu => pu.Position)
                    .FirstOrDefault(pu => pu.IsActive && pu.UserId == userId)?.Position
                    ?? throw new NotFoundException($"There is not position on which the user with id {userId} works");
            }

            throw new BadRequestException("You must specify 'positionId' or 'userId'.");
        }

        public List<DbPosition> Find(int skipCount, int takeCount, bool includeDeactivated, out int totalCount)
        {
            if (skipCount < 0)
            {
                throw new BadRequestException("Skip count can't be less than 0.");
            }

            if (takeCount <= 0)
            {
                throw new BadRequestException("Take count can't be equal or less than 0.");
            }

            var dbPositions = _provider.Positions.AsQueryable();

            if (includeDeactivated)
            {
                totalCount = _provider.Positions.Count();
            }
            else
            {
                totalCount = _provider.Positions.Count(p => p.IsActive);
                dbPositions = dbPositions.Where(p => p.IsActive);
            }

            return dbPositions.Skip(skipCount).Take(takeCount).ToList();
        }

        public bool PositionContainsUsers(Guid positionId)
        {
            return _provider.PositionUsers
                .Any(pu => pu.PositionId == positionId && pu.IsActive);
        }

        public Guid Create(DbPosition newPosition)
        {
            _provider.Positions.Add(newPosition);
            _provider.Save();

            return newPosition.Id;
        }

        public bool Edit(Guid positionId, JsonPatchDocument<DbPosition> request)
        {
            var dbPosition = _provider.Positions.FirstOrDefault(position => position.Id == positionId)
                ?? throw new NotFoundException($"Position with this id: '{positionId}' was not found.");

            request.ApplyTo(dbPosition);
            dbPosition.ModifiedAtUtc = DateTime.UtcNow;
            dbPosition.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
            _provider.Save();

            return true;
        }

        public bool DoesNameExist(string name)
        {
            return _provider.Positions.Any(p => p.Name == name);
        }

        public bool Contains(Guid positionId)
        {
            return _provider.Positions.Any(p => p.Id == positionId);
        }
    }
}