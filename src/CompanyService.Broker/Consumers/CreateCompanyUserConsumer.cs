using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;
using LT.DigitalOffice.Models.Broker.Publishing.Subscriber.Company;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
  public class CreateCompanyUserConsumer : IConsumer<ICreateCompanyUserPublish>
  {
    private readonly ICompanyUserRepository _companyUserRepository;
    private readonly IDbCompanyUserMapper _companyUserMapper;
    private readonly IGlobalCacheRepository _globalCache;
    private readonly ILogger<CreateCompanyUserConsumer> _logger;

    public CreateCompanyUserConsumer(
      ICompanyUserRepository companyUserRepository,
      IDbCompanyUserMapper companyUserMapper,
      IGlobalCacheRepository globalCache)
    {
      _companyUserRepository = companyUserRepository;
      _companyUserMapper = companyUserMapper;
      _globalCache = globalCache;
    }

    public async Task Consume(ConsumeContext<ICreateCompanyUserPublish> context)
    {
      if (!await _companyUserRepository.DoesExistAsync(context.Message.UserId))
      {
        await _companyUserRepository.CreateAsync(_companyUserMapper.Map(context.Message));

        await _globalCache.RemoveAsync(context.Message.CompanyId);
      }
      else
      {
        _logger.LogError($"Failed to save user with ID {context.Message.UserId} - user already exists.");
      }
    }
  }
}
