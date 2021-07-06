﻿using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Requests.File;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using LT.DigitalOffice.Models.Broker.Responses.File;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company
{
    public class EditCompanyCommand : IEditCompanyCommand
    {
        private readonly IAccessValidator _accessValidator;
        private readonly IRequestClient<IAddImageRequest> _rcAddImage;
        private readonly ILogger<EditCompanyCommand> _logger;
        private readonly IPatchDbCompanyMapper _mapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEditCompanyRequestValidator _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRequestClient<IUpdateSmtpCredentialsRequest> _rcUpdateSmtp;

        private void UpdateSmtp(DbCompany company, List<string> errors)
        {
            string message = "Can not update smtp credentials.";

            try
            {
                var response = _rcUpdateSmtp.GetResponse<IOperationResult<bool>>(
                    IUpdateSmtpCredentialsRequest.CreateObj(
                        host: company.Host,
                        port: company.Port,
                        enableSsl: company.EnableSsl,
                        email: company.Email,
                        password: company.Password)).Result.Message;

                if (response.IsSuccess && response.Body)
                {
                    return;
                }

                _logger.LogWarning(message, string.Join("\n", response.Errors));
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, message);
            }

            errors.Add(message);
        }

        private Guid? GetImageId(AddImageRequest logo, List<string> errors)
        {
            string logMessage = "Cannot add image.";
            string errorMessage = "Cannot change image now. Please try again later.";

            try
            {
                Guid userId = _httpContextAccessor.HttpContext.GetUserId();

                var response = _rcAddImage.GetResponse<IOperationResult<IAddImageResponse>>(
                    IAddImageRequest.CreateObj(logo.Name, logo.Content, logo.Extension, userId)).Result.Message;

                if (response.IsSuccess)
                {
                    return response.Body.Id;
                }

                errors.Add(errorMessage);

                _logger.LogWarning(string.Join("\n", response.Errors));
            }
            catch(Exception exc)
            {
                _logger.LogError(exc, logMessage);
            }

            errors.Add(errorMessage);

            return null;
        }

        public EditCompanyCommand(
            IAccessValidator accessValidator,
            IRequestClient<IAddImageRequest> rcAddImage,
            ILogger<EditCompanyCommand> logger,
            IPatchDbCompanyMapper mapper,
            ICompanyRepository companyRepository,
            IEditCompanyRequestValidator validator,
            IRequestClient<IUpdateSmtpCredentialsRequest> rcUpdateSmtp)
        {
            _accessValidator = accessValidator;
            _rcAddImage = rcAddImage;
            _logger = logger;
            _mapper = mapper;
            _companyRepository = companyRepository;
            _validator = validator;
            _rcUpdateSmtp = rcUpdateSmtp;
        }

        public OperationResultResponse<bool> Execute(JsonPatchDocument<EditCompanyRequest> request)
        {
            if (!_accessValidator.IsAdmin())
            {
                throw new ForbiddenException("Not enouth rights.");
            }

            _validator.ValidateAndThrowCustom(request);

            List<string> errors = new List<string>();

            var imageOperation = request.Operations.FirstOrDefault(o => o.path.EndsWith(nameof(EditCompanyRequest.Logo), StringComparison.OrdinalIgnoreCase));
            Guid? imageId = null;

            if (imageOperation != null)
            {
                imageId = GetImageId(JsonConvert.DeserializeObject<AddImageRequest>(imageOperation.value?.ToString()), errors);
            }

            _companyRepository.Edit(_mapper.Map(request, imageId));

            if (request.Operations.FirstOrDefault(o => o.path.EndsWith(nameof(EditCompanyRequest.Host), StringComparison.OrdinalIgnoreCase)) != null
                || request.Operations.FirstOrDefault(o => o.path.EndsWith(nameof(EditCompanyRequest.Port), StringComparison.OrdinalIgnoreCase)) != null
                || request.Operations.FirstOrDefault(o => o.path.EndsWith(nameof(EditCompanyRequest.EnableSsl), StringComparison.OrdinalIgnoreCase)) != null
                || request.Operations.FirstOrDefault(o => o.path.EndsWith(nameof(EditCompanyRequest.Email), StringComparison.OrdinalIgnoreCase)) != null
                || request.Operations.FirstOrDefault(o => o.path.EndsWith(nameof(EditCompanyRequest.Password), StringComparison.OrdinalIgnoreCase)) != null)
            {
                UpdateSmtp(_companyRepository.Get(), errors);
            }

            return new OperationResultResponse<bool>
            {
                Status = errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess,
                Body = true,
                Errors = errors
            };
        }
    }
}