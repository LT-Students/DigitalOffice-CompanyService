using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using MassTransit;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
  public class CreateCompanyUsersConsumer : IConsumer<ICreateCompanyUserRequest>
  {
    private readonly ICompanyUserRepository _companyUserRepository;
    private readonly IDbCompanyUserMapper _companyUserMapper;

    private async Task<bool> CreateAsync(ICreateCompanyUserRequest request)
    {
      await _companyUserRepository.CreateAsync(_companyUserMapper.Map(request));

      return true;
    }

    public CreateCompanyUsersConsumer(
      ICompanyUserRepository companyUserRepository,
      IDbCompanyUserMapper companyUserMapper)
    {
      _companyUserRepository = companyUserRepository;
      _companyUserMapper = companyUserMapper;
    }

    public async Task Consume(ConsumeContext<ICreateCompanyUserRequest> context)
    {
      object response = OperationResultWrapper.CreateResponse(CreateAsync, context.Message);

      await context.RespondAsync<IOperationResult<bool>>(response);
    }
  }
}
