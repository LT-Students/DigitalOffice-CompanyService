using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Kernel.RedisSupport.Helpers.Interfaces;
using LT.DigitalOffice.Models.Broker.Publishing.Subscriber.Company;
using MassTransit;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
  public class CreateCompanyUserConsumer : IConsumer<ICreateCompanyUserPublish>
  {
    private readonly ICompanyUserRepository _companyUserRepository;
    private readonly IDbCompanyUserMapper _companyUserMapper;
    private readonly IGlobalCacheRepository _globalCache;

    private async Task<bool> CreateAsync(ICreateCompanyUserPublish request)
    {
      await _companyUserRepository.CreateAsync(_companyUserMapper.Map(request));

      return true;
    }

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
      object response = OperationResultWrapper.CreateResponse(CreateAsync, context.Message);

      await _globalCache.RemoveAsync(context.Message.CompanyId);

      await context.RespondAsync<IOperationResult<bool>>(response);
    }
  }
}
