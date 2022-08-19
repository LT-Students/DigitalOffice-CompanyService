using System.ComponentModel.DataAnnotations;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests
{
  public record CreateCompanyRequest
  {
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    public string Tagline { get; set; }
    public string Contacts { get; set; }
    public ImageConsist Logo { get; set; }
  }
}
