using System;
using System.Collections.Generic;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
  [AutoInject]
  public interface IOfficeRepository
  {
    void Add(DbOffice office);

    DbOffice Get(Guid officeId);

    List<DbOffice> Find(BaseFindFilter filter, out int totalCount);

    bool Edit(Guid officeId, JsonPatchDocument<DbOffice> news);

    bool Contains(Guid officeId);
  }
}
