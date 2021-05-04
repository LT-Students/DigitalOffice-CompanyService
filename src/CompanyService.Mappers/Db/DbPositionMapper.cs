using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Db
{
    public class DbPositionMapper : IDbPositionMapper
    {
        public DbPosition Map(CreatePositionRequest value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new DbPosition
            {
                Id = Guid.NewGuid(),
                Name = value.Name,
                Description = value.Description,
                IsActive = true
            };
        }

        public DbPosition Map(PositionInfo positionInfo)
        {
            if (positionInfo == null)
            {
                throw new ArgumentNullException(nameof(positionInfo));
            }

            return new DbPosition
            {
                Id = positionInfo.Id,
                Name = positionInfo.Name,
                Description = positionInfo.Description,
                IsActive = positionInfo.IsActive
            };
        }
    }
}
