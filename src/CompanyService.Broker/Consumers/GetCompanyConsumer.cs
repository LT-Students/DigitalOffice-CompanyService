﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Broker;
using MassTransit;
using Microsoft.Extensions.Options;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
  public class GetCompanyConsumer : IConsumer<IGetCompaniesRequest>
  {
    private readonly ICompanyUserRepository _userRepository;
    private readonly IOptions<RedisConfig> _redisConfig;
    private readonly IRedisHelper _redisHelper;
    private readonly ICacheNotebook _cacheNotebook;
    private readonly ICompanyDataMapper _companyDataMapper;

    private async Task<List<CompanyData>> GetCompanyAsync(IGetCompaniesRequest request)
    {
      List<DbCompanyUser> usersInfo = await _userRepository.GetAsync(request);

      List<DbCompany> companies = usersInfo.Select(u => u.Company).Distinct().ToList();

      return companies.Select(p => _companyDataMapper.Map(p).ToList());
    }

    public GetCompanyConsumer(
      ICompanyUserRepository userRepository,
      IOptions<RedisConfig> redisConfig,
      IRedisHelper redisHelper,
      ICacheNotebook cacheNotebook,
      ICompanyDataMapper companyDataMapper)
    {
      _userRepository = userRepository;
      _redisConfig = redisConfig;
      _redisHelper = redisHelper;
      _cacheNotebook = cacheNotebook;
      _companyDataMapper = companyDataMapper;
    }

    public async Task Consume(ConsumeContext<IGetCompaniesRequest> context)
    {
      List<CompanyData> companies = await GetCompanyAsync(context.Message);

      object response = OperationResultWrapper.CreateResponse((_) => IGetCompaniesResponse.CreateObj(companies), context.Message);

      await context.RespondAsync<IOperationResult<IGetCompaniesResponse>>(response);

      if (companies != null && companies.Any() && context.Message.UsersIds != null)
      {
        string key = context.Message.UsersIds.GetRedisCacheHashCode();

        await _redisHelper.CreateAsync(Cache.Companies, key, companies, TimeSpan.FromMinutes(_redisConfig.Value.CacheLiveInMinutes));

        _cacheNotebook.Add(companies.Select(c => c.Id).ToList(), Cache.Companies, key);
      }
    }
  }
}
