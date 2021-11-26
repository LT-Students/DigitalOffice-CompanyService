using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models.Position;

namespace LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces
{
  [AutoInject]
  public interface IPositionInfoMapper
  {
    PositionInfo Map(PositionData position);
  }
}
