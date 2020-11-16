using System;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Models
{
    public class DepartmentUser
    {
        public Guid? Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PositionId { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
