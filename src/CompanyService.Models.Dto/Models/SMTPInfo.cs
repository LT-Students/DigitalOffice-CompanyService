namespace LT.DigitalOffice.CompanyService.Models.Dto.Models
{
    public record SmtpInfo
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
