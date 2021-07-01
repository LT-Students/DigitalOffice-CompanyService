using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Models
{
    public class CompanyInfo
    {
        public Guid Id { get; set; }
        public string PortalName { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string Tagline { get; set; }
        public string SiteUrl { get; set; }
        public ImageInfo Logo { get; set; }
        public List<ShortDepartmentInfo> Departments { get; set; }
        public List<PositionInfo> Positions { get; set; }
        public List<OfficeInfo> Offices { get; set; }
    }
}
