using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;

namespace LT.DigitalOffice.CompanyService.Mappers.RequestMappers.Interfaces
{
    /// <summary>
    /// Represents mapper. Provides methods for converting an object of <see cref="CreateDepartmentRequest"/>
    /// type into an object of <see cref="DbDepartment"/> type according to some rule.
    /// </summary>
    public interface IDbDepartmentMapper : IMapper<NewDepartmentRequest, DbDepartment>
    {
    }
}
