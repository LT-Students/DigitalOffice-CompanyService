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
    public class GetUsersDepartmentsUsersPositionsConsumer : IConsumer<IGetUsersDepartmentsUsersPositionsRequest>
    {
        private readonly IDepartmentUserRepository _departmentUserRepository;
        private readonly IPositionUserRepository _positionUserRepository;

        private List<DepartmentData> GetDepartments(List<Guid> userIds)
        {
            List<DepartmentData> response = new ();

            List<DbDepartmentUser> dbDepartmentUsers = _departmentUserRepository.Find(userIds);

            DepartmentData isInResponse = null;

            foreach (DbDepartmentUser dbDepartmentUser in dbDepartmentUsers)
            {
                isInResponse = response.FirstOrDefault(x => x.Id == dbDepartmentUser.DepartmentId);

                if (isInResponse != null)
                {
                    isInResponse.UserIds.Add(dbDepartmentUser.UserId);
                }
                else
                {
                    response.Add(
                        new DepartmentData(
                            dbDepartmentUser.DepartmentId,
                            dbDepartmentUser.Department.Name,
                            new List<Guid>() { dbDepartmentUser.UserId }));
                }
            }

            return response;
        }

        private List<PositionData> GetPositions(List<Guid> userIds)
        {
            List<PositionData> response = new();

            List<DbPositionUser> dbPositionsUsers = _positionUserRepository.Find(userIds);

            PositionData IsInResponse = null;

            foreach (DbPositionUser dbPositionUser in dbPositionsUsers)
            {
                IsInResponse = response.FirstOrDefault(x => x.Id == dbPositionUser.PositionId);

                if (IsInResponse != null)
                {
                    IsInResponse.UserIds.Add(dbPositionUser.UserId);
                }
                else
                {
                    response.Add(
                        new PositionData(
                            dbPositionUser.PositionId,
                            dbPositionUser.Position.Name,
                            new List<Guid>() { dbPositionUser.UserId }));
                }
            }

            return response;
        }

        private object GetResponse(IGetUsersDepartmentsUsersPositionsRequest request)
        {
            List<DepartmentData> usersDepartments = null;
            List<PositionData> usersPositions = null;

            if (request.IncludeDepartments)
            {
                usersDepartments = GetDepartments(request.UserIds);
            }

            if (request.IncludePositions)
            {
                usersPositions = GetPositions(request.UserIds);
            }

            return IGetUsersDepartmentsUsersPositionsResponse
                .CreateObj(usersDepartments, usersPositions);
        }

        public GetUsersDepartmentsUsersPositionsConsumer(
            IDepartmentUserRepository departmentUserRepository,
            IPositionUserRepository positionUserRepository)
        {
            _departmentUserRepository = departmentUserRepository;
            _positionUserRepository = positionUserRepository;
        }

        public async Task Consume(ConsumeContext<IGetUsersDepartmentsUsersPositionsRequest> context)
        {
            var response = OperationResultWrapper
                .CreateResponse(
                GetResponse,
                context.Message);

            await context.RespondAsync<IOperationResult<IGetUsersDepartmentsUsersPositionsResponse>>(response);
        }
    }
}
