using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
  [AutoInject]
  public interface IOfficeRepository
  {
    Task CreateAsync(DbOffice office);

    Task<DbOffice> GetAsync(Guid officeId);

    Task<(List<DbOffice>, int totalCount)> FindAsync(OfficeFindFilter filter);

    Task<bool> EditAsync(Guid officeId, JsonPatchDocument<DbOffice> news);

    Task<bool> DoesExistAsync(Guid officeId);
  }
}
