using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Db
{
    public class DbDepartmentUserMapper : IDbDepartmentUserMapper
    {
        public DbDepartmentUser Map(IChangeUserDepartmentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new DbDepartmentUser
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                DepartmentId = request.DepartmentId,
                IsActive = true,
                CreatedBy = request.ChangedBy,
                CreatedAtUtc = DateTime.UtcNow
            };
        }
    }
}
