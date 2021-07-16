using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using System;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Mappers.Db
{
    public class DbOfficeMapper : IDbOfficeMapper
    {
        public DbOffice Map(CreateOfficeRequest request, Guid companyId)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new DbOffice
            {
                Id = Guid.NewGuid(),
                CompanyId = companyId,
                Name = request.Name != null && request.Name.Trim().Any() ? request.Name.Trim() : null,
                City = request.City,
                Address = request.Address,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }
    }
}
