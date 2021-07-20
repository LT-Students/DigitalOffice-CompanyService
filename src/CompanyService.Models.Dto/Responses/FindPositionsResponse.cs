using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Responses
{
    public record FindPositionsResponse
    {
        public int TotalCount { get; set; }
        public List<PositionInfo> Positions { get; set; } = new();
        public List<string> Errors { get; set; } = new();
    }
}
