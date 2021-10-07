using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using MassTransit;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
  public class EditCompanyEmployeeConsumer : IConsumer<IEditCompanyEmployeeRequest>
  {
    private readonly IPositionRepository _positionRepository;
    private readonly IPositionUserRepository _positionUserRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IDepartmentUserRepository _departmentUserRepository;
    private readonly IOfficeRepository _officeRepository;
    private readonly IOfficeUserRepository _officeUserRepository;
    private readonly IDbPositionUserMapper _positionUserMapper;
    private readonly IDbDepartmentUserMapper _departmentUserMapper;
    private readonly IDbOfficeUserMapper _officeUserMapper;

    private bool ChangePosition(Guid userId, Guid? positionId, Guid modifiedBy)
    {
      if (positionId == null)
      {
        return true;
      }

      if (!_positionRepository.Contains(positionId.Value))
      {
        return false;
      }

      _positionUserRepository.Remove(userId, modifiedBy);
      return _positionUserRepository.Add(_positionUserMapper.Map(userId, positionId.Value, modifiedBy));
    }

    private bool ChangeDepartment(Guid userId, bool removeUserFromDepartment, Guid? departmentId, Guid modifiedBy)
    {
      if (departmentId == null && !removeUserFromDepartment)
      {
        return true;
      }

      if (!_departmentRepository.Contains(departmentId.Value))
      {
        return false;
      }

      _departmentUserRepository.Remove(userId, modifiedBy);

      if (!departmentId.HasValue)
      {
        return true;
      }

      return _departmentUserRepository.Add(
        new List<DbDepartmentUser>() { _departmentUserMapper.Map(userId, departmentId.Value, modifiedBy) });
    }

    private bool ChangeOffice(Guid userId, Guid? officeId, Guid modifiedBy)
    {
      if (officeId == null)
      {
        return true;
      }

      if (!_officeRepository.Contains(officeId.Value))
      {
        return false;
      }

      _officeUserRepository.Remove(userId, modifiedBy);
      return _officeUserRepository.Add(_officeUserMapper.Map(userId, officeId.Value, modifiedBy));
    }

    private (bool department, bool position, bool office) ChangeEmployee(IEditCompanyEmployeeRequest request)
    {
      return (department: ChangeDepartment(request.UserId, request.RemoveUserFromDepartment, request.DepartmentId, request.ModifiedBy),
        position: ChangePosition(request.UserId, request.PositionId, request.ModifiedBy),
        office: ChangeOffice(request.UserId, request.OfficeId, request.ModifiedBy));
    }

    public EditCompanyEmployeeConsumer(
      IPositionRepository positionRepository,
      IPositionUserRepository positionUserRepository,
      IDbPositionUserMapper positionUserMapper,
      IDepartmentRepository departmentRepository,
      IDepartmentUserRepository departmentUserRepository,
      IDbDepartmentUserMapper departmentUserMapper,
      IOfficeRepository officeRepository,
      IOfficeUserRepository officeUserRepository,
      IDbOfficeUserMapper officeUserMapper)
    {
      _positionRepository = positionRepository;
      _positionUserRepository = positionUserRepository;
      _positionUserMapper = positionUserMapper;
      _departmentRepository = departmentRepository;
      _departmentUserRepository = departmentUserRepository;
      _departmentUserMapper = departmentUserMapper;
      _officeRepository = officeRepository;
      _officeUserRepository = officeUserRepository;
      _officeUserMapper = officeUserMapper;
    }

    public async Task Consume(ConsumeContext<IEditCompanyEmployeeRequest> context)
    {
      object response = OperationResultWrapper.CreateResponse(ChangeEmployee, context.Message);

      await context.RespondAsync<IOperationResult<(bool department, bool position, bool office)>>(response);
    }
  }
}
