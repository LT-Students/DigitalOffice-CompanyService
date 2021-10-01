using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

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

    public DbPosition Get(Guid positionId)
    {
      return _provider.Positions.FirstOrDefault(d => d.Id == positionId);
    }

    public List<DbPosition> Get(List<Guid> positionsIds, bool includeUsers)
    {
      IQueryable<DbPosition> positions = _provider.Positions.Where(p => positionsIds.Contains(p.Id));

      if (includeUsers)
      {
        positions = positions.Include(p => p.Users);
      }

      return positions.ToList();
    }

    public List<DbPosition> Find(int skipCount, int takeCount, bool includeDeactivated, out int totalCount)
    {
      if (skipCount < 0)
      {
        throw new ArgumentException("Skip count can't be less than 0.");
      }

      if (takeCount < 1)
      {
        throw new ArgumentException("Take count can't be less than 1.");
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

    public bool Edit(DbPosition position, JsonPatchDocument<DbPosition> request)
    {
      if (position == null)
      {
        throw new ArgumentNullException(nameof(position));
      }

      request.ApplyTo(position);
      position.ModifiedAtUtc = DateTime.UtcNow;
      position.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
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
