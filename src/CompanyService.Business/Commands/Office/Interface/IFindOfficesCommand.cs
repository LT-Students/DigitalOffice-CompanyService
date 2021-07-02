using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Office.Interface
{
    [AutoInject]
    public interface IFindOfficesCommand
    {
        FindOfficesResponse Execute(int skipCount, int takeCount);
    }
}
