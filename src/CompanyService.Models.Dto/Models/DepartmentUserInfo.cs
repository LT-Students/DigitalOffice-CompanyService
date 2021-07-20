using System;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Models
{
    public record DepartmentUserInfo
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public float Rate { get; set; }
        public bool IsActive { get; set; }
        public ImageInfo Image { get; set; }
        public PositionInfo Position { get; set; }
    }
}
