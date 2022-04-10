using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Broker.Requests.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
using LT.DigitalOffice.Models.Broker.Common;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.CompanyService.Broker.Requests
{
  public class UserService : IUserService
  {
    private readonly IRequestClient<ICheckUsersExistence> _rcCheckUserExistence;
    private readonly ILogger<UserService> _logger;

    public UserService(
      IRequestClient<ICheckUsersExistence> rcCheckUserExistence,
      ILogger<UserService> logger)
    {
      _rcCheckUserExistence = rcCheckUserExistence;
      _logger = logger;
    }

    public async Task<List<Guid>> CheckUsersExistence(List<Guid> usersIds, List<string> errors)
    {
      return (await RequestHandler.ProcessRequest<ICheckUsersExistence, ICheckUsersExistence>(
        _rcCheckUserExistence,
        ICheckUsersExistence.CreateObj(usersIds),
        errors,
        _logger))
      ?.UserIds;
    }
  }
}
