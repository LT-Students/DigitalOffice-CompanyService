using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces
{
    [AutoInject]
    public interface IDbOfficeMapper
    {
        DbOffice Map(CreateOfficeRequest request, Guid companyId);
    }
}
