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
            DepartmentsResponse response = new ();

            var dbDepartments = _repository.FindDepartments();

            foreach (var department in dbDepartments)
            {
                List<Guid> usersIds = department.Users.Select(x => x.UserId).ToList();

                if (department.DirectorUserId != null)
                {
                    usersIds.Add((Guid)department.DirectorUserId);
                }

                List<UserData> usersData = GetUsers(usersIds, response.Errors);

                UserInfo director = null;
                if (department.DirectorUserId != null && usersData.Any())
                {
                    director = _userMapper.Map(usersData.FirstOrDefault(x => x.Id == department.DirectorUserId));
                }

                List<UserInfo> usersInfo = new();
                foreach(UserData userData in usersData)
                {
                    if (userData != null)
                    {
                        usersInfo.Add(_userMapper.Map(userData));
                    }
                }

                response.Departments.Add(_departmentMapper.Map(department, director, usersInfo));
            }

            return response;
        }
    }
}
