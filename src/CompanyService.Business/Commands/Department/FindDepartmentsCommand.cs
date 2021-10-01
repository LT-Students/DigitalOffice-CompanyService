using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Enums;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.File;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.File;
using LT.DigitalOffice.Models.Broker.Responses.User;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

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
    private readonly IHttpContextAccessor _httpContextAccessor;
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

      return await GetUsersDataFromBroker(userIds, errors);
    }

    private async Task<List<UserData>> GetUsersDataFromBroker(List<Guid> userIds, List<string> errors)
    {
      if (userIds == null || !userIds.Any())
      {
        return new();
      }

      string message = "Can not get users data. Please try again later.";
      string loggerMessage = $"Can not get users data for specific user ids:'{string.Join(",", userIds)}'.";

      try
      {
        var response = await _rcGetUsersData.GetResponse<IOperationResult<IGetUsersDataResponse>>(
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

    private List<ImageData> GetImages(List<Guid> imageIds, List<string> errors)
    {
      string logMessage = "Can not get images: {ids}.";
      string errorMessage = "Can not get images. Please try again later.";

      if (imageIds == null || !imageIds.Any())
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
      ILogger<FindDepartmentsCommand> logger,
      IHttpContextAccessor httpContextAccessor,
      IConnectionMultiplexer cache)
    {
      _repository = repository;
      _userPositionRepository = userPositionRepository;
      _departmentMapper = departmentMapper;
      _userMapper = userMapper;
      _rcGetUsersData = rcGetUsersData;
      _rcGetImages = rcGetImages;
      _logger = logger;
      _httpContextAccessor = httpContextAccessor;
      _cache = cache;
    }

    public async Task<FindResultResponse<DepartmentInfo>> Execute(int skipCount, int takeCount, bool includeDeactivated)
    {
      if (skipCount < 0)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return new()
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { "Skip count can't be less than 0." }
        };
      }

      if (takeCount < 1)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return new()
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { "Take count can't be less than 1." }
        };
      }

      FindResultResponse<DepartmentInfo> response = new(body: new());

      var dbDepartments = _repository.Find(skipCount, takeCount, includeDeactivated, out int totalCount);

      response.TotalCount = totalCount;

      Dictionary<Guid, Guid> departmentsDirectors =
        dbDepartments
          .SelectMany(d => d.Users.Where(u => u.Role == (int)DepartmentUserRole.Director)).ToDictionary(d => d.DepartmentId, d => d.UserId);

      List<UserData> usersData = await GetUsersData(
        departmentsDirectors.Values.ToList(),
        response.Errors);

      List<ImageData> images = GetImages(usersData.Where(d => d.ImageId.HasValue).Select(d => d.ImageId.Value).ToList(), response.Errors);

      foreach (var department in dbDepartments)
      {
        UserInfo director = null;
        if (departmentsDirectors.ContainsKey(department.Id) && usersData.Any())
        {
          var directorUserData = usersData.FirstOrDefault(x => x.Id == departmentsDirectors[department.Id]);

          DbPositionUser positionUser = _userPositionRepository.Get(directorUserData.Id);

          director = directorUserData == null ? null : _userMapper.Map(directorUserData, positionUser, images.FirstOrDefault(i => i.ImageId == directorUserData.ImageId));
        }

        response.Body.Add(_departmentMapper.Map(department, director));
      }

      response.Status = response.Errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess;

      return response;
    }
  }
}
