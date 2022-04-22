using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.Models.Broker.Publishing.Subscriber.Company;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
  public class CreateCompanyUserConsumer : IConsumer<ICreateCompanyUserPublish>
  {
    private readonly ICompanyUserRepository _companyUserRepository;
    private readonly IDbCompanyUserMapper _companyUserMapper;
    private readonly ILogger<CreateCompanyUserConsumer> _logger;

    public CreateCompanyUserConsumer(
      ICompanyUserRepository companyUserRepository,
      IDbCompanyUserMapper companyUserMapper,
      ILogger<CreateCompanyUserConsumer> logger)
    {
      _companyUserRepository = companyUserRepository;
      _companyUserMapper = companyUserMapper;
      _logger = logger;
    }

    public async Task Consume(ConsumeContext<ICreateCompanyUserPublish> context)
    {
      if (!(await _companyUserRepository.CreateAsync(_companyUserMapper.Map(context.Message))).HasValue)
      {
        _logger.LogError("Error while assign company to user.");
      }
    }
  }
}
