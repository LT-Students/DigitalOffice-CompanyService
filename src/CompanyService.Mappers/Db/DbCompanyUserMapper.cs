using System;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Company;

namespace LT.DigitalOffice.CompanyService.Mappers.Db
{
  public class DbCompanyUserMapper : IDbCompanyUserMapper
  {
    public DbCompanyUser Map(ICreateCompanyUserRequest request)
    {
      return new DbCompanyUser
      {
        Id = Guid.NewGuid(),
        CompanyId = request.CompanyId,
        UserId = request.UserId,
        Rate = request.Rate != null ? request.Rate : null,
        StartWorkingAt = request.StartWorkingAt != null ? request.StartWorkingAt : null,
        CreatedBy = request.CreatedBy,
        CreatedAtUtc = DateTime.UtcNow,
        IsActive = true
      };
    }
  }
}
