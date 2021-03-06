﻿using System;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Models
{
    public record BaseDepartmentInfo
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? DirectorUserId { get; set; }
    }
}
