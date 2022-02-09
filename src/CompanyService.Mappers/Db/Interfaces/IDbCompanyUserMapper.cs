﻿using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Requests.Company;

namespace LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbCompanyUserMapper
  {
    DbCompanyUser Map(ICreateCompanyUserRequest request);
  }
}