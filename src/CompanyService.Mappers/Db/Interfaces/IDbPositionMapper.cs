using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Position;
using LT.DigitalOffice.Kernel.Attributes;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces
{
    /// <summary>
    /// Represents mapper. Provides methods for converting an object of <see cref="PositionInfo"/>
    /// type into an object of <see cref="DbPosition"/> type according to some rule.
    /// </summary>
    [AutoInject]
    public interface IDbPositionMapper
    {
        DbPosition Map(CreatePositionRequest positionInfo, Guid companyId);
    }
}
