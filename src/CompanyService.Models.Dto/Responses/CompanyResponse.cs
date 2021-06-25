using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Responses
{
    public class CompanyResponse
    {
        public CompanyInfo Company { get; set; }
        public List<string> Errors { get; set; }
    }
}
