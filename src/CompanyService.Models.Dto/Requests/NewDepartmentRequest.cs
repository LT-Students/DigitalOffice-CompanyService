using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests
{
    public class NewDepartmentRequest
    {
        public Department Info { get; set; }
        public IEnumerable<DepartmentUser> Users { get; set; }
    }
}
