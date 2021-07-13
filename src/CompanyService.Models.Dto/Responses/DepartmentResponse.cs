using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Responses
{
    public record DepartmentResponse
    {
        public ShortDepartmentInfo Department { get; set; }
        public IEnumerable<DepartmentUserInfo> Users { get; set; }
        public IEnumerable<ProjectInfo> Projects { get; set; }
    }
}
