using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Requests.File;
using LT.DigitalOffice.Models.Broker.Responses.File;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company
{
    public class CreateCompanyCommand : ICreateCompanyCommand
    {
        private readonly IAccessValidator _accessValidator;
        private readonly IDbCompanyMapper _mapper;
        private readonly IRequestClient<IAddImageRequest> _rcAddImage;
        private readonly ILogger<CreateCompanyCommand> _logger;
        private readonly HttpContext _httpContext;
        private readonly ICreateCompanyRequestValidator _validator;
        private readonly ICompanyRepository _repository;

        private Guid? AddImage(AddImageRequest image, List<string> errors)
        {
            Guid creatorId = Guid.NewGuid();//_httpContext.GetUserId();
            string logMessage = "Cannot add image: name - {name}, context - {context}, extension - {extension}, creatorId - {creatorId}";
            string errorMessage = "Cannot add company logo. Please try later.";

            try
            {
                var response = _rcAddImage.GetResponse<IOperationResult<IAddImageResponse>>(
                                                IAddImageRequest.CreateObj(image.Name, image.Content, image.Extension, creatorId)).Result.Message;

                if (response.IsSuccess)
                {
                    return response.Body.Id;
                }

                _logger.LogWarning(logMessage, image.Name, image.Content, image.Extension, creatorId);
            }
            catch (Exception exc)
            {
                _logger.LogWarning(exc, logMessage, image.Name, image.Content, image.Extension, creatorId);
            }

            errors.Add(errorMessage);

            return null;
        }

        public CreateCompanyCommand(
            IAccessValidator accessValidator,
            IDbCompanyMapper mapper,
            IRequestClient<IAddImageRequest> rcAddImage,
            ILogger<CreateCompanyCommand> logger,
            IHttpContextAccessor accessor,
            ICreateCompanyRequestValidator validator,
            ICompanyRepository repository)
        {
            _accessValidator = accessValidator;
            _mapper = mapper;
            _rcAddImage = rcAddImage;
            _logger = logger;
            _httpContext = accessor.HttpContext;
            _validator = validator;
            _repository = repository;
        }

        public OperationResultResponse<Guid> Execute(CreateCompanyRequest request)
        {/*
            if (!_accessValidator.IsAdmin())
            {
                throw new ForbiddenException("Not enough rights.");
            }
            */
            _validator.ValidateAndThrowCustom(request);

            List<string> errors = new();

            DbCompany company = _mapper.Map(request, request.Logo == null ? null : AddImage(request.Logo, errors));

            _repository.Add(company);

            return new OperationResultResponse<Guid>
            {
                Status = errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess,
                Body = company.Id,
                Errors = errors
            };
        }
    }
}
