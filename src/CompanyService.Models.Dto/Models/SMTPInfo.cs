namespace LT.DigitalOffice.CompanyService.Models.Dto.Models
{
    public class SMTPInfo
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
