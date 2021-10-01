namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office
{
  public record EditOfficeRequest
  {
    public string Name { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
  }
}
