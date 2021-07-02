using System;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests
{
    public class CreateOfficeRequest
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}
