using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Business.Interfaces
{
    public interface IFindDepartmentsCommand
    {
        List<DepartmentResponse> Execute();
    }
}
