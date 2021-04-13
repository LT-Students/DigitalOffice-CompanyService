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
        public List<DepartmentResponse> Execute()
        {
            var departments = _repository.FindDepartments();

            List<DepartmentResponse> departmentsList = new();

            foreach (var department in departments)
            {
                try
                {
                    List<Guid> usersIds = new();

                    usersIds.AddRange(department.Users.Select(x => x.Id));

                    if (department.DirectorUserId != null)
                    {
                        usersIds.Add((Guid)department.DirectorUserId);
                    }

                    var usersDataResponse = _requestClient.GetResponse<IOperationResult<IGetUsersDataResponse>>(
                        IGetUsersDataRequest.CreateObj(usersIds)).Result;

                    if (usersDataResponse.Message.IsSuccess)
                    {
                        var director = _userMapper.Map(usersDataResponse.Message.Body.UsersData.First(x => x.Id == department.DirectorUserId));

                        var users = usersDataResponse.Message.Body.UsersData.Select(x => _userMapper.Map(x)).ToList();

                        departmentsList.Add(_departmentMapper.Map(department, director, users));
                    }
                }
                catch (Exception exc)
                {
                    _logger?.LogError(exc, "Exception on get user information.");
                }
            }

            return departmentsList;
        }
    }
}
