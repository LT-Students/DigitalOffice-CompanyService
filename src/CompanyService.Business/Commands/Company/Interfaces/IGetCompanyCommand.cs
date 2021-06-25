using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces
{
    [AutoInject]
    public interface IGetCompanyCommand
    {
        CompanyResponse Execute();
    }
}
