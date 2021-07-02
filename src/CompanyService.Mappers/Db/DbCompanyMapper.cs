using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Db
{
    public class DbCompanyMapper : IDbCompanyMapper
    {
        public DbCompany Map(CreateCompanyRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new DbCompany
            {
                Id = Guid.NewGuid(),
                PortalName = request.PortalName,
                CompanyName = request.CompanyName,
                SiteUrl = request.SiteUrl,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }
    }
}
