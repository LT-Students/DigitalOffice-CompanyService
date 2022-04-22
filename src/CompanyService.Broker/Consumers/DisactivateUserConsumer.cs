using System;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;
using LT.DigitalOffice.Models.Broker.Publishing;
using MassTransit;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
  public class DisactivateUserConsumer : IConsumer<IDisactivateUserPublish>
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

    public async Task Consume(ConsumeContext<IDisactivateUserPublish> context)
    {
      Guid? companyId = await _companyUserRepository.RemoveAsync(context.Message.UserId, context.Message.ModifiedBy);

      if (companyId.HasValue)
      {
        await _globalCache.RemoveAsync(companyId.Value);
      }
    }
  }
}
