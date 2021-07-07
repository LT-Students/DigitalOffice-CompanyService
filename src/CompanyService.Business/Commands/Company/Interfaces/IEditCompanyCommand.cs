using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces
{
    [AutoInject]
    public interface IEditCompanyCommand
    {
        OperationResultResponse<bool> Execute(JsonPatchDocument<EditCompanyRequest> request);
    }
}
