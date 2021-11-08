﻿using System;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.CompanyService.Data
{
  public class CompanyRepository : ICompanyRepository
  {
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CompanyRepository(
      IDataProvider provider,
      IHttpContextAccessor httpContextAccessor)
    {
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task CreateAsync(DbCompany company)
    {
      if (company == null)
      {
        return;
      }

      if (await _provider.Companies.AnyAsync())
      {
        return;
      }

      _provider.Companies.Add(company);
      await _provider.SaveAsync();
    }

    public async Task<DbCompany> GetAsync()
    {
      return await _provider.Companies.FirstOrDefaultAsync();
    }

    public async Task EditAsync(JsonPatchDocument<DbCompany> request)
    {
      if (request == null)
      {
        return;
      }

      var company = await _provider.Companies.FirstOrDefaultAsync();

      if (company == null)
      {
        return;
      }

      request.ApplyTo(company);
      company.ModifiedAtUtc = DateTime.UtcNow;
      company.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();

      await _provider.SaveAsync();
    }
  }
}
