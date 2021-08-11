using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Position;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Mappers.Db
{
    public class DbPositionMapper : IDbPositionMapper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbPositionMapper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbPosition Map(CreatePositionRequest value, Guid companyId)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new DbPosition
            {
                Id = Guid.NewGuid(),
                CompanyId = companyId,
                Name = value.Name,
                Description = value.Description != null && value.Description.Trim().Any() ? value.Description.Trim() : null,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow,
                CreatedBy = _httpContextAccessor.HttpContext.GetUserId()
            };
        }
    }
}
