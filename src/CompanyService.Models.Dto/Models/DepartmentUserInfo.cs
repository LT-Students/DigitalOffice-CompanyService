﻿namespace LT.DigitalOffice.CompanyService.Models.Dto.Models
{
    public record DepartmentUserInfo : UserInfo
    {
        public float Rate { get; set; }
        public bool IsActive { get; set; }
        public string EndTime { get; set; }
        public string StartTime { get; set; }
        public ImageInfo Image { get; set; }
        public PositionInfo Position { get; set; }
    }
}
