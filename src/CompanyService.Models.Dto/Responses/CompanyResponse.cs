using System;
using System.Collections.Generic;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Responses
{
  public record CompanyResponse
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Tagline { get; set; }
    public string Contacts { get; set; }
    public ImageConsist Logo { get; set; }
    public List<OfficeInfo> Offices { get; set; }
  }
}
