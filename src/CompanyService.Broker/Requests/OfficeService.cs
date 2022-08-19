using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Broker.Requests.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
using LT.DigitalOffice.Models.Broker.Models.Office;
using LT.DigitalOffice.Models.Broker.Requests.Office;
using LT.DigitalOffice.Models.Broker.Responses.Office;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.CompanyService.Broker.Requests
{
  public class OfficeService : IOfficeService
  {
    private readonly IRequestClient<IGetOfficesRequest> _rcGetOffices;
    private readonly ILogger<OfficeService> _logger;

    public OfficeService(
      IRequestClient<IGetOfficesRequest> rcGetOffices,
      ILogger<OfficeService> logger)
    {
      _rcGetOffices = rcGetOffices;
      _logger = logger;
    }

    public async Task<List<OfficeData>> GetOfficesAsync(List<string> errors)
    {
      return (await RequestHandler.ProcessRequest<IGetOfficesRequest, IGetOfficesResponse>(
        _rcGetOffices,
        IGetOfficesRequest.CreateObj(null),
        errors,
        _logger))
      ?.Offices;
    }
  }
}
