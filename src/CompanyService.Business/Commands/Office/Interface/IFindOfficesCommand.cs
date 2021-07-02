using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Office.Interface
{
    [AutoInject]
    public interface IFindOfficesCommand
    {
        OperationResultResponse<List<OfficeInfo>> Execute(int skipCount, int takeCount);
    }
}
