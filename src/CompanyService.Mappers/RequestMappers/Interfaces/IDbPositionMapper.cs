using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Mappers.RequestMappers.Interfaces
{
    /// <summary>
    /// Represents mapper. Provides methods for converting an object of <see cref="Position"/>
    /// type into an object of <see cref="DbPosition"/> type according to some rule.
    /// </summary>
    [AutoInject]
    public interface IDbPositionMapper : IMapper<Position, DbPosition>
    {
    }
}
