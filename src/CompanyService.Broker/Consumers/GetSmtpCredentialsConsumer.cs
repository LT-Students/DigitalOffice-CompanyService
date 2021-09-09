using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using MassTransit;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
  public class GetSmtpCredentialsConsumer : IConsumer<IGetSmtpCredentialsRequest>
  {
    private ICompanyRepository _repository;

    private object GetSmtpCredentials(object arg)
    {
      DbCompany company = _repository.Get();

      return IGetSmtpCredentialsResponse.CreateObj(
        host: company.Host,
        port: company.Port,
        enableSsl: company.EnableSsl,
        email: company.Email,
        password: company.Password);
    }

    public GetSmtpCredentialsConsumer(ICompanyRepository repository)
    {
      _repository = repository;
    }

    public async Task Consume(ConsumeContext<IGetSmtpCredentialsRequest> context)
    {
      object response = OperationResultWrapper.CreateResponse(GetSmtpCredentials, context.Message);

      await context.RespondAsync<IOperationResult<IGetSmtpCredentialsResponse>>(response);
    }
  }
}
