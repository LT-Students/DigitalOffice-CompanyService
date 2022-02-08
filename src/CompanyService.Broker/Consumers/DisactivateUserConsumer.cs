﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;
using LT.DigitalOffice.Models.Broker.Common;
using MassTransit;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
  public class DisactivateUserConsumer : IConsumer<IDisactivateUserRequest>
  {
    private readonly ICompanyUserRepository _companyUserRepository;
    private readonly IGlobalCacheRepository _globalCache;

    public DisactivateUserConsumer(
      ICompanyUserRepository companyUserRepository,
      IGlobalCacheRepository globalCache)
    {
      _companyUserRepository = companyUserRepository;
      _globalCache = globalCache;
    }

    public async Task Consume(ConsumeContext<IDisactivateUserRequest> context)
    {
      await _companyUserRepository.RemoveAsync(context.Message.UserId, context.Message.ModifiedBy);

      DbCompanyUser user = await _companyUserRepository.GetAsync(context.Message.UserId);
      await _globalCache.RemoveAsync(user.CompanyId);
    }
  }
}
