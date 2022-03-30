using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
  [AutoInject]
  public interface IContractSubjectRepository
  {
    Task<Guid?> CreateAsync(DbContractSubject contractSubject);
    Task<bool> EditAsync(Guid contractSubjectId, JsonPatchDocument<DbContractSubject> request);
    Task<List<DbContractSubject>> GetAllSubjectsAsync(Guid companyId);
    Task<DbContractSubject> GetAsync(Guid contractSubjectId);
  }
}
