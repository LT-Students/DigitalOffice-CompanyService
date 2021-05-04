using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests
{
    public class NewDepartmentRequest
    {
        public BaseDepartmentInfo Info { get; set; }
        public IEnumerable<DepartmentUserInfo> Users { get; set; }
    }
}
