﻿using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
    public class OfficeInfoMapper : IOfficeInfoMapper
    {
        public OfficeInfo Map(DbOffice office)
        {
            if (office == null)
            {
                throw new ArgumentNullException(nameof(office));
            }

            return new OfficeInfo
            {
                Id = office.Id,
                Name = office.Name.Trim(),
                City = office.City.Trim(),
                Address = office.Address.Trim(),
                IsActive = office.IsActive
            };
        }
    }
}
