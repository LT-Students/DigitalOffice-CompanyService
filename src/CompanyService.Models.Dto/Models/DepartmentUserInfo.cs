using System;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Models
{
    public record DepartmentUserInfo
    {
        public Guid UserId { get; set; }
        public Guid PositionId { get; set; }
    }
}
