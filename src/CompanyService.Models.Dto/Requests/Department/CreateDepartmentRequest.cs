using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department
{
    public record CreateDepartmentRequest
    {
        public BaseDepartmentInfo Info { get; set; }
        public IEnumerable<Guid> Users { get; set; }
    }
}
