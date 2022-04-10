using System;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.CompanyService.Mappers.Db
{
  public class DbCompanyUserMapper : IDbCompanyUserMapper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DbCompanyUserMapper(
      IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public DbCompanyUser Map(CreateCompanyUserRequest request)
    {
      return request is null 
        ? default
        : new DbCompanyUser
        {
          Id = Guid.NewGuid(),
          CompanyId = request.CompanyId,
          UserId = request.UserId,
          ContractSubjectId = request.ContractSubjectId,
          ContractTermType = (int)request.ContractTermType,
          Rate = request.Rate,
          StartWorkingAt = request.StartWorkingAt,
          EndWorkingAt = request.EndWorkingAt,
          Probation = request.Probation,
          CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
          CreatedAtUtc = DateTime.UtcNow,
          IsActive = true
        };
    }

    public DbCompanyUser Map(ICreateCompanyUserRequest request)
    {
      return new DbCompanyUser
      {
        Id = Guid.NewGuid(),
        CompanyId = request.CompanyId,
        UserId = request.UserId,
        ContractSubjectId = request.ContractSubjectId,
        ContractTermType = (int)request.ContractTermType,
        Rate = request.Rate,
        StartWorkingAt = request.StartWorkingAt,
        EndWorkingAt = request.EndWorkingAt,
        Probation = request.Probation,
        CreatedBy = request.CreatedBy,
        CreatedAtUtc = DateTime.UtcNow,
        IsActive = true
      };
    }
  }
}
