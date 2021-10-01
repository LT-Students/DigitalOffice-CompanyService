using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces
{
    [AutoInject]
    public interface IFindPositionsCommand
    {
        FindResultResponse<PositionInfo> Execute(int skipCount, int takeCount, bool includeDeactivated);
    }
}
