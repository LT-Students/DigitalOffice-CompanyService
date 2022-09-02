using System;
using System.Threading.Tasks;
using DigitalOffice.Models.Broker.Publishing;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
  public class ActivateCompanyUserConsumer : IConsumer<IActivateUserPublish>
  {
    private readonly ICompanyUserRepository _repository;
    private readonly IGlobalCacheRepository _globalCache;
    private readonly ILogger<ActivateCompanyUserConsumer> _logger;

    public ActivateCompanyUserConsumer(
      ICompanyUserRepository companyUserRepository,
      IGlobalCacheRepository globalCache,
      ILogger<ActivateCompanyUserConsumer> logger)
    {
      _repository = companyUserRepository;
      _globalCache = globalCache;
      _logger = logger;
    }

    public async Task Consume(ConsumeContext<IActivateUserPublish> context)
    {
      Guid? companyId = await _repository.ActivateAsync(context.Message);

      if (companyId.HasValue)
      {
        await _globalCache.RemoveAsync(companyId.Value);

        _logger.LogInformation("UserId '{UserId}' activated in companyId '{CompanyId}'", context.Message.UserId, companyId);
      }
    }
  }
}
