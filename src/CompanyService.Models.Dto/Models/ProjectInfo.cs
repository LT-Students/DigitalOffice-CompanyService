﻿using System;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Models
{
    public record ProjectInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
    }
}
