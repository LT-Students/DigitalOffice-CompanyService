using LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.File;
using LT.DigitalOffice.Models.Broker.Requests.Project;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.File;
using LT.DigitalOffice.Models.Broker.Responses.Project;
using LT.DigitalOffice.Models.Broker.Responses.User;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Department
{
    public class GetDepartmentCommand : IGetDepartmentCommand
    {
        private readonly ILogger<GetDepartmentCommand> _logger;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IPositionUserRepository _positionUserRepository;
        private readonly IDepartmentResponseMapper _departmentResponseMapper;
        private readonly IRequestClient<IGetImagesRequest> _rcImages;
        private readonly IRequestClient<IGetUsersDataRequest> _rcDepartmentUsers;
        private readonly IRequestClient<IGetDepartmentProjectsRequest> _rcDepartmentProject;

        private List<UserData> GetUsersData(List<Guid> userIds, List<string> errors)
        {
            string message = "Can not get users data. Please try again later.";
            string loggerMessage = $"Can not get users data for specific user ids:'{string.Join(",", userIds)}'.";

            if (userIds == null && userIds.Any())
            {
                return new();
            }

            try
            {
                var response = _rcDepartmentUsers.GetResponse<IOperationResult<IGetUsersDataResponse>>(
                    IGetUsersDataRequest.CreateObj(userIds)).Result;

                if (response.Message.IsSuccess)
                {
                    return response.Message.Body.UsersData;
                }

                _logger.LogWarning(loggerMessage + "Reasons: {Errors}", string.Join("\n", response.Message.Errors));
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, loggerMessage);
            }

            errors.Add(message);

            return new();
        }

        private List<ProjectData> GetProjectsData(Guid departmentId, List<string> errors)
        {
            string message = "Can not get projects data. Please try again later.";
            string loggerMessage = $"Can not get projects data for specific department id '{departmentId}'.";

            try
            {
                var response = _rcDepartmentProject.GetResponse<IOperationResult<IGetDepartmentProjectsResponse>>(
                    IGetDepartmentProjectsRequest.CreateObj(departmentId)).Result;

                if (response.Message.IsSuccess)
                {
                    return response.Message.Body.Projects;
                }

                _logger.LogWarning(loggerMessage + "Reasons: {Errors}", string.Join("\n", response.Message.Errors));
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, loggerMessage);
            }

            errors.Add(message);

            return new();
        }

        private List<ImageData> GetUsersImage(List<Guid> imageIds, List<string> errors)
        {
            string message = "Can not get users avatar. Please try again later.";
            string loggerMessage = $"Can not get users avatar by specific image ids '{string.Join(",", imageIds)}.";

            if (imageIds == null && imageIds.Any())
            {
                return new();
            }

            try
            {
                var response = _rcImages.GetResponse<IOperationResult<IGetImagesResponse>>(
                    IGetImagesRequest.CreateObj(imageIds)).Result;

                if (response.Message.IsSuccess)
                {
                    return response.Message.Body.Images;
                }

                _logger.LogWarning(loggerMessage + "Reasons: {Errors}", string.Join("\n", response.Message.Errors));
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, loggerMessage);
            }

            errors.Add(message);

            return new();
        }

        public GetDepartmentCommand(
            IDepartmentRepository departmentRepository,
            IPositionUserRepository positionUserRepository,
            IDepartmentResponseMapper departmentResponseMapper,
            IRequestClient<IGetImagesRequest> rcImages,
            IRequestClient<IGetUsersDataRequest> rcDepartmentUsers,
            IRequestClient<IGetDepartmentProjectsRequest> rcDepartmentProject,
            ILogger<GetDepartmentCommand> logger)
        {
            _logger = logger;
            _rcImages = rcImages;
            _rcDepartmentUsers = rcDepartmentUsers;
            _rcDepartmentProject = rcDepartmentProject;
            _departmentRepository = departmentRepository;
            _positionUserRepository = positionUserRepository;
            _departmentResponseMapper = departmentResponseMapper;
        }

        public OperationResultResponse<DepartmentResponse> Execute(GetDepartmentFilter filter)
        {
            List<string> errors = new();

            DbDepartment dbDepartment = _departmentRepository.GetDepartment(filter);

            List<Guid> userIds = new();
            if (dbDepartment.DirectorUserId.HasValue)
            {
                userIds.Add(dbDepartment.DirectorUserId.Value);
            }

            List<DbPositionUser> dbPositionUsers = new();
            if (filter.IsIncludeUsers)
            {
                userIds.AddRange(dbDepartment.Users.Select(u => u.UserId).ToList());
                dbPositionUsers = _positionUserRepository.Find(userIds);
            }

            List<UserData> usersData = null;
            List<ImageData> userImages = null;
            if (dbDepartment.DirectorUserId.HasValue || filter.IsIncludeUsers)
            {
                usersData = GetUsersData(userIds, errors);
                userImages = GetUsersImage(usersData.Where(
                    us => us.ImageId.HasValue).Select(us => us.ImageId.Value).ToList(), errors);
            }

            List<ProjectData> projectsInfo = null;
            if (filter.IsIncludeProjects)
            {
                projectsInfo = GetProjectsData(dbDepartment.Id, errors);
            }

            return new OperationResultResponse<DepartmentResponse>
            {
                Body = _departmentResponseMapper.Map(dbDepartment, usersData, dbPositionUsers, userImages, projectsInfo, filter),
                Status = errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess,
                Errors = errors
            };
        }
    }
}
