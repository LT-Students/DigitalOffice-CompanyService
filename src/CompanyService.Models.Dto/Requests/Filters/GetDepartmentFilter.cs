
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests.Filters
{
    public record GetDepartmentFilter
    {
        [FromQuery(Name ="departmentid")]
        public Guid DepartmentId { get; set; }
        public bool? IncludeUsers { get; set; }
        public bool? IncludeProjects { get; set; }

        public bool IsIncludeUsers => IncludeUsers.HasValue && IncludeUsers.Value;
        public bool IsIncludeProjects => IncludeProjects.HasValue && IncludeProjects.Value;
    }
}
