using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class GetUsersDepartmentsAndPositionsConsumer : IConsumer<IGetUsersDepartmentsAndPositionsRequest>
    {
        private readonly IDepartmentUserRepository _departmentUserRepository;
        private readonly IPositionUserRepository _positionUserRepository;

        private List<UsersDepartment> GetDepartments(List<Guid> userIds)
        {
            List<UsersDepartment> response = new ();

            List<DbDepartmentUser> dbDepartmentUsers = _departmentUserRepository.Find(userIds);

            foreach (DbDepartmentUser dbDepartmentUser in dbDepartmentUsers)
            {
                var departmentIsInresponse = response.FirstOrDefault(x => x.Id == dbDepartmentUser.DepartmentId);

                if (departmentIsInresponse == null)
                {
                    List<Guid> g = new List<Guid>() { dbDepartmentUser.UserId };

                    response.Add(new UsersDepartment(dbDepartmentUser.DepartmentId, dbDepartmentUser.Department.Name, g));

                }
                else
                {
                    response.FirstOrDefault(x => x.Id == dbDepartmentUser.DepartmentId).UserIds.Add(dbDepartmentUser.UserId);
                }
            }
            return response;
        }

        private List<UsersPosition> GetPositions(List<Guid> userIds)
        {
            List<UsersPosition> response = new();

            List<DbPositionUser> dbPositionsUsers = _positionUserRepository.Find(userIds);

            foreach (DbPositionUser dbPositionUser in dbPositionsUsers)
            {
                var positionIsInresponse = response.FirstOrDefault(x => x.Id == dbPositionUser.PositionId);

                if (positionIsInresponse == null)
                {
                    List<Guid> g = new List<Guid>() { dbPositionUser.UserId };

                    response.Add(new UsersPosition(dbPositionUser.PositionId, dbPositionUser.Position.Name, g));

                }
                else
                {
                    response.FirstOrDefault(x => x.Id == dbPositionUser.PositionId).UserIds.Add(dbPositionUser.UserId);
                }
            }
            return response;
        }

        private object GetResponse(List<Guid> userIds)
        {
            return IGetUsersDepartmentsAndPositionsResponse.CreateObj(GetDepartments(userIds), GetPositions(userIds));
        }

        public GetUsersDepartmentsAndPositionsConsumer(
            IDepartmentUserRepository departmentUserRepository,
            IPositionUserRepository positionUserRepository)
        {
            _departmentUserRepository = departmentUserRepository;
            _positionUserRepository = positionUserRepository;

        }

        public async Task Consume(ConsumeContext<IGetUsersDepartmentsAndPositionsRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(GetResponse, context.Message.UserIds);

            await context.RespondAsync<IOperationResult<IGetUsersDepartmentsAndPositionsResponse>>(response);
        }
    }
}
