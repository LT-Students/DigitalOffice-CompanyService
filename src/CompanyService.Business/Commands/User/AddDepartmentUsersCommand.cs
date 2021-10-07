using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.CompanyService.Business.Commands.User.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.User;
using LT.DigitalOffice.CompanyService.Validation.Users.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.CompanyService.Business.Commands.User
{
  public class AddDepartmentUsersCommand : IAddDepartmetUsersCommand
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAccessValidator _accessValidator;
    private readonly IAddDepartmentUsersRequestValidator _validator;
    private readonly IDbDepartmentUserMapper _mapper;
    private readonly IDepartmentUserRepository _repository;

    public AddDepartmentUsersCommand(
      IHttpContextAccessor httpContextAccessor,
      IAccessValidator accessValidator,
      IAddDepartmentUsersRequestValidator validator,
      IDbDepartmentUserMapper mapper,
      IDepartmentUserRepository repository)
    {
      _httpContextAccessor = httpContextAccessor;
      _accessValidator = accessValidator;
      _validator = validator;
      _mapper = mapper;
      _repository = repository;
    }

    public async Task<OperationResultResponse<bool>> Execute(AddDepartmentUsersRequest request)
    {
      Guid senderUserId = _httpContextAccessor.HttpContext.GetUserId();

      if (!_accessValidator.HasRights(Rights.AddEditRemoveDepartments) &&
        _repository.IsDepartmentDirector(request.DepartmentId, senderUserId))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        return new OperationResultResponse<bool>
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { "Not enough rights." }
        };
      }

      ValidationResult validationResult = await _validator.ValidateAsync(request);

      if (!validationResult.IsValid)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return new OperationResultResponse<bool>
        {
          Status = OperationResultStatusType.Failed,
          Errors = validationResult.Errors.Select(vf => vf.ErrorMessage).ToList()
        };
      }

      OperationResultResponse<bool> response = new();

      _repository.Remove(request.UsersIds, senderUserId);

      response.Body = _repository.Add(
        request.UsersIds.Select(x => _mapper.Map(x, request.DepartmentId, senderUserId)).ToList());

      _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

      response.Status = OperationResultStatusType.FullSuccess;

      if (!response.Body)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        response.Status = OperationResultStatusType.Failed;
      }

      return response;
    }
  }
}
