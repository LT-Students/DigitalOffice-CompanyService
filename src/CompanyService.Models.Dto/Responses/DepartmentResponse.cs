using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Responses
{
    public record DepartmentResponse
    {
        public DepartmentInfo Department { get; set; }
        public IEnumerable<UserInfo> Users { get; set; }
        public IEnumerable<ProjectInfo> Projects { get; set; }
    }
}
