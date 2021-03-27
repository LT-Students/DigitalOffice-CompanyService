using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces
{
    /// <summary>
    /// Represents mapper. Provides methods for converting an object of <see cref="DbDepartment"/>
    /// type into an object of <see cref="Department"/> type according to some rule.
    /// </summary>
    public interface IDepartmentResponseMapper : IMapper<DbDepartment, Department>
    {
    }
}
