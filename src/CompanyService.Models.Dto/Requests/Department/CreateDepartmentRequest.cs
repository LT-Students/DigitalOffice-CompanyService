using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department
{
    public record CreateDepartmentRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? DirectorUserId { get; set; }
        public IEnumerable<Guid> Users { get; set; }
    }
}
