using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Dto
{
    public class DepartmentRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Guid> UsersIds { get; set; }
        public Guid CompanyId { get; set; }
    }
}
