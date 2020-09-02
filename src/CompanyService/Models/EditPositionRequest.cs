using System;

namespace LT.DigitalOffice.CompanyService.Models
{
    public class EditPositionRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}