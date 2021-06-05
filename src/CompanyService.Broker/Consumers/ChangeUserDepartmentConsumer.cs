using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using MassTransit;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class ChangeUserDepartmentConsumer : IConsumer<IChangeUserDepartmentRequest>
    {
        private readonly IDepartmentUserRepository _repository;
        private readonly IDbDepartmentUserMapper _mapper;

        private object ChangeDepartment(IChangeUserDepartmentRequest request)
        {
            _repository.Remove(request.UserId);
            bool isSuccess = _repository.Add(_mapper.Map(request.DepartmentId, request.UserId));

            return isSuccess;
        }

        public ChangeUserDepartmentConsumer(
            IDepartmentUserRepository repository,
            IDbDepartmentUserMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<IChangeUserDepartmentRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(ChangeDepartment, context.Message);

            await context.RespondAsync<IOperationResult<bool>>(response);
        }
    }
}
