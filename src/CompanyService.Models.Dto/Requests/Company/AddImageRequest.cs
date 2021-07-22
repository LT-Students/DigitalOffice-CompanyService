namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests
{
    public record AddImageRequest
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string Extension { get; set; }
    }
}
