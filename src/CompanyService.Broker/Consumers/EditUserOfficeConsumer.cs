using System;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using MassTransit;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
  public class EditUserOfficeConsumer : IConsumer<IEditUserOfficeRequest>
  {
    private readonly IOfficeRepository _officeRepository;
    private readonly IOfficeUserRepository _officeUserRepository;
    private readonly IDbOfficeUserMapper _officeUserMapper;
    private readonly ICacheNotebook _cacheNotebook;

    private async Task<bool> ChangeOffice(IEditUserOfficeRequest request)
    {
      if (request.OfficeId.HasValue && !await _officeRepository.DoesExistAsync(request.OfficeId.Value))
      {
        return false;
      }

      Guid? removedOfficeId = await _officeUserRepository.RemoveAsync(request.UserId, request.ModifiedBy);

      if (removedOfficeId.HasValue)
      {
        await _cacheNotebook.RemoveAsync(removedOfficeId.Value);
      }

      return request.OfficeId == null ? true : await _officeUserRepository.CreateAsync(_officeUserMapper.Map(request));
    }

    public EditUserOfficeConsumer(
      IOfficeRepository officeRepository,
      IOfficeUserRepository officeUserRepository,
      IDbOfficeUserMapper officeUserMapper,
      ICacheNotebook cacheNotebook)
    {
      _officeRepository = officeRepository;
      _officeUserRepository = officeUserRepository;
      _officeUserMapper = officeUserMapper;
      _cacheNotebook = cacheNotebook;
    }

    public async Task Consume(ConsumeContext<IEditUserOfficeRequest> context)
    {
      object response = OperationResultWrapper.CreateResponse(ChangeOffice, context.Message);

      await context.RespondAsync<IOperationResult<bool>>(response);
    }
  }
}
