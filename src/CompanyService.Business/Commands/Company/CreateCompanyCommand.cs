using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Requests.User;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company
{
    public class CreateCompanyCommand : ICreateCompanyCommand
    {
        private readonly IDbCompanyMapper _mapper;
        private readonly ILogger<ICreateCompanyCommand> _logger;
        private readonly ICreateCompanyRequestValidator _validator;
        private readonly ICompanyRepository _repository;
        private readonly IRequestClient<ICreateAdminRequest> _rcCreateAdmin;

        private bool CreateAdmin(AdminInfo info, List<string> errors)
        {
            string message = "Can not create admin.";

            try
            {
                var response = _rcCreateAdmin.GetResponse<IOperationResult<bool>>(
                    ICreateAdminRequest.CreateObj(info.FirstName, info.MiddleName, info.LastName, info.Email, info.Login, info.Password)).Result.Message;

                if (response.IsSuccess && response.Body)
                {
                    return true;
                }

                errors.Add(message);

                _logger.LogWarning(message, string.Join("\n", response.Errors));
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, message);

                errors.Add(message);
            }

            return false;
        }


        public CreateCompanyCommand(
            IDbCompanyMapper mapper,
            ILogger<ICreateCompanyCommand> logger,
            ICreateCompanyRequestValidator validator,
            ICompanyRepository repository,
            IRequestClient<ICreateAdminRequest> rcCreateAdmin)
        {
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
            _repository = repository;
            _rcCreateAdmin = rcCreateAdmin;
        }

        public OperationResultResponse<Guid> Execute(CreateCompanyRequest request)
        {
            List<string> errors = new();

            if (_repository.Get() != null)
            {
                throw new BadRequestException("Company already exists");
            }

            _validator.ValidateAndThrowCustom(request);

            if(!CreateAdmin(request.AdminInfo, errors))
            {
                return new OperationResultResponse<Guid>
                {
                    Status = OperationResultStatusType.Failed,
                    Errors = errors
                };
            }

            DbCompany company = _mapper.Map(request);

            _repository.Add(company);

            return new OperationResultResponse<Guid>
            {
                Status = OperationResultStatusType.FullSuccess,
                Body = company.Id
            };
        }
    }
}
