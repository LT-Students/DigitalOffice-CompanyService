using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Models.Broker.Common;
using MassTransit;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
  internal class DisactivateUserConsumer : IConsumer<IDisactivateUserRequest>
  {
    private readonly ICompanyUserRepository _companyUserRepository;

    public DisactivateUserConsumer(
      ICompanyUserRepository companyUserRepository)
    {
      _companyUserRepository = companyUserRepository;
    }

    public async Task Consume(ConsumeContext<IDisactivateUserRequest> context)
    {
      await Task.WhenAll(
        _companyUserRepository.RemoveAsync(context.Message.UserId, context.Message.ModifiedBy));
    }
  }
}
