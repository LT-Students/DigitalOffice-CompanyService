using LT.DigitalOffice.Kernel.Attributes;
using System;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
    [AutoInject]
    public interface ICompanyChangesRepository
    {
        void Add(Guid companyId, Guid? changedBy, string changes);
    }
}
