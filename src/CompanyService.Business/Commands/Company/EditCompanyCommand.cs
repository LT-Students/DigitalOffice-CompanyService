using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Requests.File;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICompanyChangesRepository _companyChangesRepository;

        private Guid? GetImageId(AddImageRequest logo, List<string> errors)
        {
            string logMessage = "Cannot add image.";
            string errorMessage = "Cannot change image now. Please try again later.";

            try
            {
                Guid userId = Guid.NewGuid();//_httpContextAccessor.HttpContext.GetUserId();

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
            ICompanyRepository companyRepository)
        {
            _accessValidator = accessValidator;
            _rcAddImage = rcAddImage;
            _logger = logger;
            _mapper = mapper;
            _companyRepository = companyRepository;
        }

        public OperationResultResponse<bool> Execute(JsonPatchDocument<EditCompanyRequest> request)
        {
            if (!_accessValidator.IsAdmin())
            {
                throw new ForbiddenException("Not enouth rights.");
            }

            List<string> errors = new List<string>();

            var imageOperation = request.Operations.FirstOrDefault(o => o.path.EndsWith(nameof(EditCompanyRequest.Logo), StringComparison.OrdinalIgnoreCase));
            Guid? imageId = null;

            if (imageOperation != null)
            {
                imageId = GetImageId(JsonConvert.DeserializeObject<AddImageRequest>(imageOperation.value?.ToString()), errors);
            }

            _companyRepository.Edit(_mapper.Map(request, imageId));

            return new OperationResultResponse<bool>
            {
                Status = errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess,
                Body = true,
                Errors = errors
            };
        }
    }
}
