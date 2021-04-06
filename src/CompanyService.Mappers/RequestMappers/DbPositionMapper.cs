﻿using LT.DigitalOffice.CompanyService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.RequestMappers
{
    public class DbPositionMapper : IDbPositionMapper
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
    }
}
