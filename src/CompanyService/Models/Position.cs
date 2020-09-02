using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models
{
    public class Position
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<Guid> UserIds { get; set; }
    }
}