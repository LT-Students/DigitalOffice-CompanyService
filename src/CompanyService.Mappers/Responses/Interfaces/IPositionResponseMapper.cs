using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces
{
    /// <summary>
    /// Represents mapper. Provides methods for converting an object of <see cref="DbPosition"/>
    /// type into an object of <see cref="PositionResponse"/> type according to some rule.
    /// </summary>
    [AutoInject]
    public interface IPositionResponseMapper
    {
        PositionResponse Map(DbPosition dbPosition);
    }
}
