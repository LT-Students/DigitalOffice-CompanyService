using System;

namespace LT.DigitalOffice.CompanyService.Models.Dto
{
    public class EditCompanyRequest
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}