﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.CompanyService.Data
{
  public class CompanyUserRepository : ICompanyUserRepository
  {
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CompanyUserRepository(
      IDataProvider provider,
      IHttpContextAccessor httpContextAccessor)
    {
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Guid?> CreateAsync(DbCompanyUser dbCompanyUser)
    {
      if (dbCompanyUser is null)
      {
        return null;
      }

      _provider.CompanysUsers.Add(dbCompanyUser);
      await _provider.SaveAsync();

      return dbCompanyUser.Id;
    }

    public async Task<bool> DoesExistAsync(Guid userId)
    {
      return await _provider.CompanysUsers.AnyAsync(u => u.UserId == userId);
    }

    public async Task<bool> EditAsync(EditCompanyUserRequest request)
    {
      if (request is null)
      {
        return false;
      }

      var companyUser = await _provider.CompanysUsers.FirstOrDefaultAsync(r => r.UserId == request.UserId);
      companyUser.Rate = request.Rate;
      companyUser.StartWorkingAt = request.StartWorkingAt;
      companyUser.ModifiedAtUtc = DateTime.UtcNow;
      companyUser.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      await _provider.SaveAsync();

      return true;
    }

    public async Task<List<DbCompanyUser>> GetAsync(IGetCompanyUserRequest request)
    {
      if (request is null)
      {
        return null;
      }

      return await _provider.CompanysUsers
        .Include(cu => cu.Company)
        .Where(u => u.IsActive && request.usersIds.Contains(u.UserId))
        .ToListAsync();
    }

    public async Task RemoveAsync(Guid userId, Guid removedBy)
    {
      DbCompanyUser user = await _provider.CompanysUsers.FirstOrDefaultAsync(u => u.UserId == userId && u.IsActive);

      if (user != null)
      {
        user.IsActive = false;
        user.ModifiedAtUtc = DateTime.UtcNow;
        user.ModifiedBy = removedBy;
        await _provider.SaveAsync();
      }
    }
  }
}

