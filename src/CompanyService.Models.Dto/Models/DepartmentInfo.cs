using System;
using System.Collections.Generic;
using System.Text;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Models
{
    public class DepartmentInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? DirectorUserId { get; set; }
    }
}
