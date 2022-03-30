using System;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.CompanyService.Mappers.Db
{
  public class DbContractSubjectMapper : IDbContractSubjectMapper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DbContractSubjectMapper(
      IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public DbContractSubject Map(CreateContractSubjectRequest request)
    {
      if (request is null)
      {
        return null;
      }

      return new DbContractSubject
      {
        Id = Guid.NewGuid(),
        CompanyId = request.CompanyId,
        Name = request.Name,
        Description = request.Description,
        IsActive = true,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = DateTime.UtcNow
      };
    }
  }
}
