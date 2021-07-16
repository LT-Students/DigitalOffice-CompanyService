using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces
{
    [AutoInject]
    public interface IPatchDbCompanyMapper
    {
        JsonPatchDocument<DbCompany> Map(JsonPatchDocument<EditCompanyRequest> request, Guid? imageId);
    }
}
