﻿using System;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.CompanyService.Mappers.Db
{
  public class DbCompanyMapper : IDbCompanyMapper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public DbCompanyMapper(
      IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public DbCompany Map(CreateCompanyRequest request)
    {
      if (request is null)
      {
        return null;
      }

      return new DbCompany
      {
        Id = Guid.NewGuid(),
        Name = request.Name,
        Description = request.Description,
        Tagline = request.Tagline,
        Contacts = request.Contacts,
        CreatedAtUtc = DateTime.UtcNow,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        IsActive = true,
        LogoContent = request.Logo?.Content,
        LogoExtension = request.Logo?.Extension
      };
    }
  }
}
