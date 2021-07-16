namespace LT.DigitalOffice.CompanyService.Models.Dto.Models
{
    public record AdminInfo
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
