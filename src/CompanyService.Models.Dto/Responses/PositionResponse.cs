using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Dto
{
    public class PositionResponse
    {
        public PositionInfo Info { get; set; }
        public IEnumerable<Guid> UserIds { get; set; }
    }
}