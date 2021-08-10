using LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Enums;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.File;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.File;
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
        private readonly IPositionUserRepository _userPositionRepository;
        private readonly IDepartmentInfoMapper _departmentMapper;
        private readonly IDepartmentUserInfoMapper _userMapper;
        private readonly IRequestClient<IGetUsersDataRequest> _rcGetUsersData;
        private readonly IRequestClient<IGetImagesRequest> _rcGetImages;
        private readonly ILogger<FindDepartmentsCommand> _logger;

        private List<UserData> GetUsers(List<Guid> usersIds, List<string> errors)
        {
            List<UserData> users = new();
            string errorMessage = $"Can not get users info for users '{usersIds}'. Please try again later.";

            if (!usersIds.Any())
            {
                return users;
            }

            try
            {
                var usersDataResponse = _rcGetUsersData.GetResponse<IOperationResult<IGetUsersDataResponse>>(
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

        private List<ImageData> GetImages(List<Guid> imageIds, List<string> errors)
        {
            string logMessage = "Can not get images: {ids}.";
            string errorMessage = "Can not get images. Please try again later.";

            if (!imageIds.Any())
            {
                return new();
            }

            try
            {
                var response = _rcGetImages.GetResponse<IOperationResult<IGetImagesResponse>>(
                    IGetImagesRequest.CreateObj(imageIds)).Result.Message;

                if (response.IsSuccess)
                {
                    return response.Body.Images;
                }

                _logger.LogWarning(logMessage + "Reason: {Errors}", string.Join(", ", imageIds), string.Join("\n", response.Errors));
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, logMessage, string.Join(", ", imageIds));
            }

            errors.Add(errorMessage);

            return new();
        }

        public FindDepartmentsCommand(
            IDepartmentRepository repository,
            IPositionUserRepository userPositionRepository,
            IDepartmentInfoMapper departmentMapper,
            IDepartmentUserInfoMapper userMapper,
            IRequestClient<IGetUsersDataRequest> rcGetUsersData,
            IRequestClient<IGetImagesRequest> rcGetImages,
            ILogger<FindDepartmentsCommand> logger)
        {
            _repository = repository;
            _userPositionRepository = userPositionRepository;
            _departmentMapper = departmentMapper;
            _userMapper = userMapper;
            _rcGetUsersData = rcGetUsersData;
            _rcGetImages = rcGetImages;
            _logger = logger;
        }

        public FindResultResponse<DepartmentInfo> Execute(int skipCount, int takeCount, bool includeDeactivated)
        {
            FindResultResponse<DepartmentInfo> response = new(body: new());

            var dbDepartments = _repository.FindDepartments(skipCount, takeCount, includeDeactivated, out int totalCount);

            response.TotalCount = totalCount;

            Dictionary<Guid, Guid> departmentsDirectors =
                dbDepartments
                    .SelectMany(d => d.Users.Where(u => u.Role == (int)DepartmentUserRole.Director)).ToDictionary(d => d.DepartmentId, d => d.UserId);

            List<UserData> usersData = GetUsers(
                departmentsDirectors.Values.ToList(),
                response.Errors);

            List<ImageData> images = GetImages(usersData.Where(d => d.ImageId.HasValue).Select(d => d.ImageId.Value).ToList(), response.Errors);

            foreach (var department in dbDepartments)
            {
                UserInfo director = null;
                if (departmentsDirectors.ContainsKey(department.Id) && usersData.Any())
                {
                    var directorUserData = usersData.FirstOrDefault(x => x.Id == departmentsDirectors[department.Id]);

                    DbPositionUser positionUser = _userPositionRepository.Get(directorUserData.Id, includePosition: true);

                    director = directorUserData == null ? null : _userMapper.Map(directorUserData, positionUser, images.FirstOrDefault(i => i.ImageId == directorUserData.ImageId));
                }

                response.Body.Add(_departmentMapper.Map(department, director));
            }

            response.Status = response.Errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess;

            return response;
        }
    }
}
