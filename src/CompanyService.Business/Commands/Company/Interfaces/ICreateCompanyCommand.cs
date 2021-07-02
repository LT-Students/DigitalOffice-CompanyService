using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using System;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces
{
    [AutoInject]
    public interface ICreateCompanyCommand
    {
        OperationResultResponse<Guid> Execute(CreateCompanyRequest request);
    }
}
