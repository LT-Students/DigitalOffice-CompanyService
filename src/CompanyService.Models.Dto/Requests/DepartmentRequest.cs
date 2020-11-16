using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests
{
    public class DepartmentRequest
    {
        public DepartmentInfo Info { get; set; }
        public IEnumerable<Guid> UsersIds { get; set; }
    }
}
