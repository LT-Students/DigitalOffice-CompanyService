using System;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department
{
    public record EditDepartmentRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid DirectorId { get; set; }
        public bool IsActive { get; set; }
    }
}
