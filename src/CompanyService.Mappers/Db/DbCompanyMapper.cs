using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Db
{
    public class DbCompanyMapper : IDbCompanyMapper
    {
        public DbCompany Map(CreateCompanyRequest request, Guid? logoId)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new DbCompany
            {
                Id = Guid.NewGuid(),
                LogoId = logoId,
                Name = request.Name,
                Description = request.Description,
                Tagline = request.Tagline,
                SiteUrl = request.SiteUrl,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }
    }
}
