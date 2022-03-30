namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests
{
  public record CreateCompanyRequest
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public string Tagline { get; set; }
    public string Contacts { get; set; }
    public AddImageRequest Logo { get; set; }
  }
}
