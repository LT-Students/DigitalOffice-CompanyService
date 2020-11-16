using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Dto
{
    public class Position
    {
        public PositionInfo Info { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<Guid> UserIds { get; set; }
    }
}