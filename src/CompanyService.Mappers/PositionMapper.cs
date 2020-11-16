﻿using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto;
using System;
using System.Linq;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;

namespace LT.DigitalOffice.CompanyService.Mappers
{
    public class PositionMapper : IMapper<PositionInfo, DbPosition>, IMapper<EditPositionRequest, DbPosition>, IMapper<DbPosition, Position>
    {
        public DbPosition Map(PositionInfo value)
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

        public DbPosition Map(EditPositionRequest value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new DbPosition
            {
                Id = value.Id,
                Name = value.Info.Name,
                Description = value.Info.Description,
            };
        }

        public Position Map(DbPosition value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new Position
            {
                Info = new PositionInfo
                {
                    Name = value.Name,
                    Description = value.Description
                },
                UserIds = value.Users?.Select(x => x.UserId).ToList(),
                IsActive = value.IsActive
            };
        }
    }
}