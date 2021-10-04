using LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Enums;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.Image;
using LT.DigitalOffice.Models.Broker.Requests.Project;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.Image;
using LT.DigitalOffice.Models.Broker.Responses.Project;
using LT.DigitalOffice.Models.Broker.Responses.User;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    private readonly IRequestClient<IGetProjectsRequest> _rcGetProjects;
    private readonly IConnectionMultiplexer _cache;

    private async Task<List<UserData>> GetUsersData(List<Guid> userIds, List<string> errors)
    {
      if (userIds == null || !userIds.Any())
      {
        return new();
      }

      RedisValue valueFromCache = await _cache.GetDatabase(Cache.Users).StringGetAsync(userIds.GetRedisCacheHashCode());

      if (valueFromCache.HasValue)
      {
        return JsonConvert.DeserializeObject<List<UserData>>(valueFromCache.ToString());
      }

      return await GetUsersDataThroughBroker(userIds, errors);
    }

    private async Task<List<UserData>> GetUsersDataThroughBroker(List<Guid> userIds, List<string> errors)
    {
      if (userIds == null || !userIds.Any())
      {
        return new();
      }

      string message = "Can not get users data. Please try again later.";
      string loggerMessage = $"Can not get users data for specific user ids:'{string.Join(",", userIds)}'.";

      try
      {
        var response = await _rcDepartmentUsers.GetResponse<IOperationResult<IGetUsersDataResponse>>(
          IGetUsersDataRequest.CreateObj(userIds));

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

      return null;
    }

    private async Task<List<ProjectData>> GetProjectsDatas(Guid departmentId, List<string> errors)
    {
      RedisValue projectsFromCache = await _cache.GetDatabase(Cache.Projects).StringGetAsync(departmentId.GetRedisCacheHashCode().ToString());

      if (projectsFromCache.HasValue)
      {
        (List<ProjectData> projects, int _) = JsonConvert.DeserializeObject<(List<ProjectData>, int)>(projectsFromCache);

        return projects;
      }

      return await GetProjectsDatasThroughBroker(departmentId, errors);
    }

    private async Task<List<ProjectData>> GetProjectsDatasThroughBroker(Guid departmentId, List<string> errors)
    {
      string message = "Can not get projects data. Please try again later.";
      string loggerMessage = $"Can not get projects data for specific department id '{departmentId}'.";

      try
      {
        var response = await _rcGetProjects.GetResponse<IOperationResult<IGetProjectsResponse>>(
          IGetProjectsRequest.CreateObj(departmentId: departmentId));

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

    private async Task<List<ImageData>> GetUsersImage(List<Guid> imagesIds, List<string> errors)
    {
      if (imagesIds == null || !imagesIds.Any())
      {
        return new();
      }

      string message = "Can not get users avatar. Please try again later.";
      string loggerMessage = $"Can not get users avatar by specific image ids '{string.Join(",", imagesIds)}.";

      try
      {
        var response = await _rcImages.GetResponse<IOperationResult<IGetImagesResponse>>(
          IGetImagesRequest.CreateObj(imagesIds, ImageSource.User));

        if (response.Message.IsSuccess)
        {
          return response.Message.Body.ImagesData;
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
      IRequestClient<IGetProjectsRequest> rcGetProjects,
      IConnectionMultiplexer cache,
      ILogger<GetDepartmentCommand> logger)
    {
      _logger = logger;
      _cache = cache;
      _rcImages = rcImages;
      _rcDepartmentUsers = rcDepartmentUsers;
      _rcGetProjects = rcGetProjects;
      _departmentRepository = departmentRepository;
      _positionUserRepository = positionUserRepository;
      _departmentResponseMapper = departmentResponseMapper;
    }

    public async Task<OperationResultResponse<DepartmentResponse>> Execute(GetDepartmentFilter filter)
    {
      List<string> errors = new();

      DbDepartment dbDepartment = _departmentRepository.Get(filter);

      Guid? directorId = dbDepartment.Users.FirstOrDefault(u => u.Role == (int)DepartmentUserRole.Director)?.Id;

      List<Guid> userIds = new();
      if (filter.IsIncludeUsers)
      {
        userIds.AddRange(dbDepartment.Users.Select(u => u.UserId).ToList());
      }
      else if (directorId.HasValue)
      {
        userIds.Add(directorId.Value);
      }

      List<DbPositionUser> dbPositionUsers = _positionUserRepository.Get(userIds);

      List<UserData> usersData = null;
      List<ImageData> userImages = null;
      if (directorId.HasValue || filter.IsIncludeUsers)
      {
        usersData = await GetUsersData(userIds, errors);
        userImages = await GetUsersImage(usersData.Where(
          us => us.ImageId.HasValue).Select(us => us.ImageId.Value).ToList(), errors);
      }

      List<ProjectData> projectsDatas = null;
      if (filter.IsIncludeProjects)
      {
        projectsDatas = await GetProjectsDatas(dbDepartment.Id, errors);
      }

      return new OperationResultResponse<DepartmentResponse>
      {
        Body = _departmentResponseMapper.Map(dbDepartment, usersData, dbPositionUsers, userImages, projectsDatas, filter),
        Status = errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess,
        Errors = errors
      };
    }
  }
}
