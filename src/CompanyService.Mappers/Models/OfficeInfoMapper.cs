using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Models.Office;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
  public class OfficeInfoMapper : IOfficeInfoMapper
  {
    public OfficeInfo Map(OfficeData office)
    {
      if (office == null)
      {
        return null;
      }

      return new OfficeInfo
      {
        Id = office.Id,
        Name = office.Name,
        City = office.City,
        Address = office.Address,
        Longitude = office.Longitude,
        Latitude = office.Latitude
      };
    }
  }
}
