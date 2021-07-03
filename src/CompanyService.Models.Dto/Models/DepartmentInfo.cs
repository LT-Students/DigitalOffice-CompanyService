using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Models
{
    public record DepartmentInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public UserInfo Director { get; set; }
        public List<UserInfo> Users { get; set; }
    }
}
