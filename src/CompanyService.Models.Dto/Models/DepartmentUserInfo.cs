using System;
using System.Collections.Generic;
using System.Text;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Models
{
    public class DepartmentUserInfo
    {
        public Guid UserId { get; set; }
        public Guid PositionId { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
