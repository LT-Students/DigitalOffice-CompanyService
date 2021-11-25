using System;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.CompanyService.Mappers.Db
{
  public class DbCompanyUserMapper : IDbCompanyUserMapper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DbCompanyUserMapper(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public DbCompanyUser Map(ICreateCompanyUserRequest request)
    {
      return new DbCompanyUser
      {
        Id = Guid.NewGuid(),
        CompanyId = request.CompanyId,
        UserId = request.UserId,
        Rate = request.Rate != null ? request.Rate : null,
        StartWorkingAt = request.StartWorkingAt != null ? request.StartWorkingAt : null,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = request.CreatedAtUtc,
        IsActive = true
      };
    }
  }
}
