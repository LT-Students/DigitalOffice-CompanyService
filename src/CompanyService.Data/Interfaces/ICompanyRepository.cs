using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
    [AutoInject]
    public interface ICompanyRepository
    {
        void Add(DbCompany company);

        DbCompany Get(bool full);
    }
}
