using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using MassTransit;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class ChangeUserDepartmentConsumer : IConsumer<IChangeUserDepartmentRequest>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDepartmentUserRepository _departmentUserRepository;
        private readonly IDbDepartmentUserMapper _mapper;

        private object ChangeDepartment(IChangeUserDepartmentRequest request)
        {
            if (!_departmentRepository.Contains(request.DepartmentId))
            {
                throw new BadRequestException($"No department with Id {request.DepartmentId}.");
            }

            _departmentUserRepository.Remove(request.UserId, request.ChangedBy);
            bool isSuccess = _departmentUserRepository.Add(_mapper.Map(request));

            return isSuccess;
        }

        public ChangeUserDepartmentConsumer(
            IDepartmentRepository departmentRepository,
            IDepartmentUserRepository departmentUserRepository,
            IDbDepartmentUserMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _departmentUserRepository = departmentUserRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<IChangeUserDepartmentRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(ChangeDepartment, context.Message);

            await context.RespondAsync<IOperationResult<bool>>(response);
        }
    }
}
