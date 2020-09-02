using System;

namespace LT.DigitalOffice.CompanyService.Models
{
    public class EditCompanyRequest
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}