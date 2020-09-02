using System;

namespace LT.DigitalOffice.CompanyService.Models
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
