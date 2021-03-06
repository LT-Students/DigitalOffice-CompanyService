﻿using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.Kernel.Attributes;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces
{
    /// <summary>
    /// Represents mapper. Provides methods for converting an object of <see cref="CreateDepartmentRequest"/>
    /// type into an object of <see cref="DbDepartment"/> type according to some rule.
    /// </summary>
    [AutoInject]
    public interface IDbDepartmentMapper
    {
        DbDepartment Map(CreateDepartmentRequest newDepartmentRequest, Guid companyId);
    }
}
