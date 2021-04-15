using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System;
using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using System.Linq;
using MassTransit;
using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.CompanyService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.UserService.Models.Broker.Models;
using LT.DigitalOffice.Kernel.Exceptions.Models;

namespace LT.DigitalOffice.CompanyService.Business
{
    public class FindDepartmentsCommand : IFindDepartmentsCommand
    {
        private readonly IDepartmentRepository _repository;
        private readonly IDepartmentResponseMapper _departmentMapper;
        private readonly IUserMapper _userMapper;
        private readonly IRequestClient<IGetUsersDataRequest> _requestClient;
        private readonly ILogger<FindDepartmentsCommand> _logger;

        public FindDepartmentsCommand(
            IDepartmentRepository repository,
            IDepartmentResponseMapper departmentMapper,
            IUserMapper userMapper,
            IRequestClient<IGetUsersDataRequest> requestClient,
            ILogger<FindDepartmentsCommand> logger)
        {
            _repository = repository;
            _departmentMapper = departmentMapper;
            _userMapper = userMapper;
            _requestClient = requestClient;
            _logger = logger;
        }

        private List<UserData> GetUsers(List<Guid> usersIds)
        {
            List<UserData> users = new();

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
                    _logger.LogWarning($"Can not get users. Reason: '{string.Join(',', usersDataResponse.Message.Errors)}'");
                }
            }
            catch (Exception exc)
            {
                _logger?.LogError(exc, "Exception on get user information.");
            }

            return users;
        }

        public List<DepartmentResponse> Execute()
        {
            var departments = _repository.FindDepartments();

            List<DepartmentResponse> departmentsList = new();

            foreach (var department in departments)
            {
                User director = null;
                var users = GetUsers(department.Users.Select(x => x.UserId).ToList());

                if (department.DirectorUserId != null && users.Any())
                {
                    director = _userMapper.Map(users.First(x => x.Id == department.DirectorUserId));
                }

                departmentsList.Add(_departmentMapper.Map(department, director, users.Select( _userMapper.Map).ToList()));
            }

            return departmentsList;
        }
    }
}
