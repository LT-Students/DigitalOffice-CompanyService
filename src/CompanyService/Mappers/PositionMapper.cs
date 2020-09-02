﻿using LT.DigitalOffice.CompanyService.Database.Entities;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models;
using System;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Mappers
{
    public class PositionMapper : IMapper<AddPositionRequest, DbPosition>, IMapper<EditPositionRequest, DbPosition>, IMapper<DbPosition, Position>
    {
        public DbPosition Map(EditPositionRequest value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new DbPosition
            {
                Id = value.Id,
                Name = value.Name,
                Description = value.Description
            };
        }

        public DbPosition Map(AddPositionRequest value)
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

        public Position Map(DbPosition value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new Position
            {
                Name = value.Name,
                Description = value.Description,
                UserIds = value.UserIds?.Select(x => x.UserId).ToList(),
                IsActive = value.IsActive
            };
        }
    }
}