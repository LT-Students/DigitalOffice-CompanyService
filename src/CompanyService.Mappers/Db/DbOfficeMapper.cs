using System;
using System.Linq;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.CompanyService.Mappers.Db
{
  public class DbOfficeMapper : IDbOfficeMapper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DbOfficeMapper(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public DbOffice Map(CreateOfficeRequest request, Guid companyId)
    {
      if (request == null)
      {
        return null;
      }

      return new DbOffice
      {
        Id = Guid.NewGuid(),
        CompanyId = companyId,
        Name = request.Name != null && request.Name.Trim().Any() ? request.Name.Trim() : null,
        City = request.City.Trim(),
        Address = request.Address.Trim(),
        CreatedAtUtc = DateTime.UtcNow,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        IsActive = true
      };
    }
  }
}
