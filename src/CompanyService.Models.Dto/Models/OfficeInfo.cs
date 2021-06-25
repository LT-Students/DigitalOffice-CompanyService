using System;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Models
{
    public class OfficeInfo
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}
