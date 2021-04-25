using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers
{
    public class PositionResponseMapper : IPositionResponseMapper
    {
        public PositionResponse Map(DbPosition value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new PositionResponse
            {
                Info = new PositionInfo
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
