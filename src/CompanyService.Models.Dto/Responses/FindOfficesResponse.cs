using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Responses
{
    public class FindOfficesResponse
    {
        public int TotalCount { get; set; }
        public List<OfficeInfo> Offices { get; set; }
        public List<string> Errors { get; set; }
    }
}
