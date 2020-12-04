using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto;
using System;
using System.Linq;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;

namespace LT.DigitalOffice.CompanyService.Mappers
{
    public class PositionMapper : IMapper<Position, DbPosition>, IMapper<DbPosition, PositionResponse>
    {
        public DbPosition Map(Position value)
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
                IsActive = value.IsActive
            };
        }

        public PositionResponse Map(DbPosition value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new PositionResponse
            {
                Info = new Position
                {
                    Name = value.Name,
                    Description = value.Description,
                    IsActive = value.IsActive
                },
                UserIds = value.Users?.Select(x => x.UserId).ToList(),
            };
        }
    }
}