using LT.DigitalOffice.CompanyService.Models.Dto.Models;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests
{
    public record CreateCompanyRequest
    {
        public string PortalName { get; set; }
        public SMTPInfo SMTP { get; set; }
        public string CompanyName { get; set; }
        public string SiteUrl { get; set; }
        public AdminInfo AdminInfo { get; set; }
    }
}
