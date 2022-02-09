﻿using LT.DigitalOffice.CompanyService.Mappers.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Models.Broker.Models.Company;

namespace LT.DigitalOffice.CompanyService.Mappers.Data
{
  public class CompanyUserDataMapper : ICompanyUserDataMapper
  {
    public CompanyUserData Map(DbCompanyUser dbCompanyUser)
    {
      if (dbCompanyUser is null)
      {
        return null;
      }

      return new(
        dbCompanyUser.UserId,
        dbCompanyUser.Rate,
        dbCompanyUser.StartWorkingAt,
        dbCompanyUser.CreatedAtUtc);
    }
  }
}