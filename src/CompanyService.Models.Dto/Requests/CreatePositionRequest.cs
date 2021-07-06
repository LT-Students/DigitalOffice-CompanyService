namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests
{
    public record CreatePositionRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
