using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Responses
{
    public record DepartmentsResponse
    {
        public int TotalCount { get; set; }
        public List<DepartmentInfo> Departments { get; set; } = new();
        public List<string> Errors { get; set; } = new();
    }
}
