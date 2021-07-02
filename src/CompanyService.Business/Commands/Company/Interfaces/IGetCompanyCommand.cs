using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces
{
    [AutoInject]
    public interface IGetCompanyCommand
    {
        OperationResultResponse<CompanyInfo> Execute();
    }
}
