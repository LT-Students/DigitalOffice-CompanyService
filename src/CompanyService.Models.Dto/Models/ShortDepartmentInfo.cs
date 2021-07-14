using System;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Models
{
    public record ShortDepartmentInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DepartmentUserInfo Director { get; set; }
    }
}
