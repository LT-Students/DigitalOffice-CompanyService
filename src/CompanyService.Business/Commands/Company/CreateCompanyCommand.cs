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
using LT.DigitalOffice.Models.Broker.Requests.Message;
using LT.DigitalOffice.Models.Broker.Requests.User;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company
{
    public class CreateCompanyCommand : ICreateCompanyCommand
    {
        private readonly IDbCompanyMapper _mapper;
        private readonly ILogger<ICreateCompanyCommand> _logger;
        private readonly ICreateCompanyRequestValidator _validator;
        private readonly ICompanyRepository _repository;
        private readonly IRequestClient<ICreateSMTPRequest> _rcCreateSMTP;
        private readonly IRequestClient<ICreateAdminRequest> _rcCreateAdmin;

        private void CreateSMTP(SMTPInfo info)
        {
            string message = "Can not create smtp.";

            try
            {
                var response = _rcCreateSMTP.GetResponse<IOperationResult<bool>>(
                    ICreateSMTPRequest.CreateObj(info.Host, info.Port, info.EnableSsl, info.Email, info.Password)).Result.Message;

                if (response.IsSuccess && response.Body)
                {
                    return;
                }
            }
            catch(Exception exc)
            {
                _logger.LogError(exc, message);

                throw;
            }
            _logger.LogError(message);

            throw new InternalServerException(message);
        }

        private void CreateAdmin(AdminInfo info)
        {
            string message = "Can not create admin.";

            try
            {
                var response = _rcCreateAdmin.GetResponse<IOperationResult<bool>>(
                    ICreateAdminRequest.CreateObj(info.FirstName, info.MiddleName, info.LastName, info.Email, info.Login, info.Password)).Result.Message;

                if (response.IsSuccess && response.Body)
                {
                    return;
                }
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, message);

                throw;
            }
            _logger.LogError(message);

            throw new InternalServerException(message);
        }


        public CreateCompanyCommand(
            IDbCompanyMapper mapper,
            ILogger<ICreateCompanyCommand> logger,
            ICreateCompanyRequestValidator validator,
            ICompanyRepository repository,
            IRequestClient<ICreateSMTPRequest> rcCreateSMTP,
            IRequestClient<ICreateAdminRequest> rcCreateAdmin)
        {
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
            _repository = repository;
            _rcCreateAdmin = rcCreateAdmin;
            _rcCreateSMTP = rcCreateSMTP;
        }

        public OperationResultResponse<Guid> Execute(CreateCompanyRequest request)
        {
            _validator.ValidateAndThrowCustom(request);

            CreateSMTP(request.SMTP);
            CreateAdmin(request.AdminInfo);

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
