using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
  public class ContractSubjectRepository : IContractSubjectRepository
  {
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ContractSubjectRepository(
      IDataProvider provider,
      IHttpContextAccessor httpContextAccessor)
    {
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Guid?> CreateAsync(DbContractSubject contractSubject)
    {
      if (contractSubject is null)
      {
        return null;
      }

      _provider.ContractSubjects.Add(contractSubject);
      await _provider.SaveAsync();

      return contractSubject.Id;
    }

    public Task<bool> EditAsync(Guid contractSubjectId, JsonPatchDocument<DbContractSubject> request)
    {
      throw new NotImplementedException();
    }

    public async Task<List<DbContractSubject>> GetAsync(Guid companyId)
    {
      return await _provider.ContractSubjects.Where(cs => cs.CompanyId == companyId).ToListAsync();
    }
  }
}
