using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Models.Position;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
  public class PositionInfoMapper : IPositionInfoMapper
  {
    public PositionInfo Map(PositionData position)
    {
      if (position == null)
      {
        return null;
      }

      return new PositionInfo
      {
        Id = position.Id,
        Name = position.Name
      };
    }
  }
}
