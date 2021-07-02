using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
    public class PositionInfoMapper : IPositionInfoMapper
    {
        public PositionInfo Map(DbPosition position)
        {
            if (position == null)
            {
                throw new ArgumentNullException(nameof(position));
            }

            return new PositionInfo
            {
                Id = position.Id,
                Name = position.Name,
                Description = position.Description,
                IsActive = position.IsActive
            };
        }
    }
}
