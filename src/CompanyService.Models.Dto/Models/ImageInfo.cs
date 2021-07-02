﻿using System;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Models
{
    public class ImageInfo
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Extension { get; set; }
    }
}
