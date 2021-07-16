using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
    [AutoInject]
    public interface ICompanyRepository
    {
        void Add(DbCompany company);

        DbCompany Get(GetCompanyFilter filter = null);

        void Edit(JsonPatchDocument<DbCompany> request);
    }
}
