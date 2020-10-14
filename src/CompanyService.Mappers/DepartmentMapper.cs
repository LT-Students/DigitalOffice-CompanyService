using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.Kernel.Exceptions;
using System;

namespace CompanyService.Mappers
{
    public class DepartmentMapper : IMapper<DepartmentRequest, DbDepartment>
    {
        public DbDepartment Map(DepartmentRequest value)
        {
            if (value == null)
            {
                throw new BadRequestException();
            }

            return new DbDepartment
            {
                Id = Guid.NewGuid(),
                Name = value.Name,
                Description = value.Description,
                IsActive = true,
                CompanyId = value.CompanyId,
              //  UserIds = value.UsersIds
            };
        }
    }
}
