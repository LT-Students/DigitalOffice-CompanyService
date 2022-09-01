using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject.Filters;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
  [AutoInject]
  public interface IContractSubjectRepository
  {
    Task CreateAsync(DbContractSubject contractSubject);
    Task<bool> EditAsync(Guid contractSubjectId, JsonPatchDocument<DbContractSubject> request);
    Task<(List<DbContractSubject> dbContractSubjects, int totalCount)> FindAsync(FindContractSubjectFilter filter);
    Task<DbContractSubject> GetAsync(Guid contractSubjectId);
    Task<bool> DoesExistAsync(Guid contractSubjectId);
    Task<bool> DoesNameExistAsync(string name);
  }
}
