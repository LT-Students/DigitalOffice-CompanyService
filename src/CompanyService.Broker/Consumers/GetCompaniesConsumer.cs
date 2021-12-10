using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Kernel.RedisSupport.Configurations;
using LT.DigitalOffice.Kernel.RedisSupport.Constants;
using LT.DigitalOffice.Kernel.RedisSupport.Extensions;
using LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;
using LT.DigitalOffice.Models.Broker.Models.Company;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using MassTransit;
using Microsoft.Extensions.Options;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
  public class GetCompaniesConsumer : IConsumer<IGetCompaniesRequest>
  {
    private readonly ICompanyRepository _repository;
    private readonly IOptions<RedisConfig> _redisConfig;
    private readonly IRedisHelper _redisHelper;
    private readonly ICacheNotebook _cacheNotebook;
    private readonly ICompanyDataMapper _companyDataMapper;

    private async Task<List<CompanyData>> GetCompaniesAsync(IGetCompaniesRequest request)
    {
      List<DbCompany> dbCompanies = await _repository.GetAsync(request);

      return dbCompanies.Select(p => _companyDataMapper.Map(p)).ToList();
    }

    public GetCompaniesConsumer(
      ICompanyRepository repository,
      IOptions<RedisConfig> redisConfig,
      IRedisHelper redisHelper,
      ICacheNotebook cacheNotebook,
      ICompanyDataMapper companyDataMapper)
    {
      _repository = repository;
      _redisConfig = redisConfig;
      _redisHelper = redisHelper;
      _cacheNotebook = cacheNotebook;
      _companyDataMapper = companyDataMapper;
    }

    public async Task Consume(ConsumeContext<IGetCompaniesRequest> context)
    {
      List<CompanyData> companies = await GetCompaniesAsync(context.Message);

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
