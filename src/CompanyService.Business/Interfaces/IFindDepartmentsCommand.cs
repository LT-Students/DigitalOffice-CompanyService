using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Business.Interfaces
{
    public interface IFindDepartmentsCommand
    {
        List<DepartmentResponse> Execute();
    }
}
