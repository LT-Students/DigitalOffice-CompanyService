using LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.User;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Department
{
    public class FindDepartmentsCommand : IFindDepartmentsCommand
    {
        private readonly IDepartmentRepository _repository;
        private readonly IDepartmentInfoMapper _departmentMapper;
        private readonly IUserInfoMapper _userMapper;
        private readonly IRequestClient<IGetUsersDataRequest> _requestClient;
        private readonly ILogger<FindDepartmentsCommand> _logger;

        public FindDepartmentsCommand(
            IDepartmentRepository repository,
            IDepartmentInfoMapper departmentMapper,
            IUserInfoMapper userMapper,
            IRequestClient<IGetUsersDataRequest> requestClient,
            ILogger<FindDepartmentsCommand> logger)
        {
            _repository = repository;
            _departmentMapper = departmentMapper;
            _userMapper = userMapper;
            _requestClient = requestClient;
            _logger = logger;
        }

        private List<UserData> GetUsers(List<Guid> usersIds, List<string> errors)
        {
            List<UserData> users = new();
            string errorMessage = $"Can not get users info for users '{usersIds}'. Please try again later.";

            try
            {
                var usersDataResponse = _requestClient.GetResponse<IOperationResult<IGetUsersDataResponse>>(
                    IGetUsersDataRequest.CreateObj(usersIds)).Result;

                if (usersDataResponse.Message.IsSuccess)
                {
                    users = usersDataResponse.Message.Body.UsersData;
                }
                else
                {
                    _logger?.LogWarning(
                        $"Can not get users. Reason:{Environment.NewLine}{string.Join('\n', usersDataResponse.Message.Errors)}.");

                    errors.Add(errorMessage);
                }
            }
            catch (Exception exc)
            {
                _logger?.LogError(exc, errorMessage);

                errors.Add(errorMessage);
            }

            return users;
        }

        public DepartmentsResponse Execute()
        {
            List<string> errors = new();

            var dbDepartments = _repository.FindDepartments();

            List<DepartmentInfo> departments = new();

            foreach (var department in dbDepartments)
            {
                UserInfo director = null;
                List<UserData> usersData = GetUsers(department.Users.Select(x => x.UserId).ToList(), errors);

                if (department.DirectorUserId != null && usersData.Any())
                {
                    UserData user = usersData.FirstOrDefault(x => x.Id == department.DirectorUserId);

                    director = user == null ? null : _userMapper.Map(user);
                }

                departments.Add(_departmentMapper.Map(department, director, usersData.Select( _userMapper.Map).ToList()));
            }

            return new DepartmentsResponse
            {
                Departments = departments,
                Errors = errors
            };
        }
    }
}
