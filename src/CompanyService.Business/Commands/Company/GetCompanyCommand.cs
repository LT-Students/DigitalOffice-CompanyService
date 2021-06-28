using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.File;
using LT.DigitalOffice.Models.Broker.Responses.File;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company
{
    public class GetCompanyCommand : IGetCompanyCommand
    {
        private readonly ICompanyRepository _repository;
        private readonly ILogger<GetCompanyCommand> _logger;
        private readonly ICompanyInfoMapper _companyInfoMapper;
        private readonly IImageInfoMapper _imageInfoMapper;
        private readonly IRequestClient<IGetImageRequest> _rcGetFile;

        private ImageInfo GetImage(Guid imageId, List<string> errors)
        {
            string logMessage = "Cannot get image with id: {id}";
            string errorMessage = $"Cannot get image with id: {imageId}. Please try later.";

            try
            {
                var response = _rcGetFile.GetResponse<IOperationResult<IGetImageResponse>>(
                    IGetImageRequest.CreateObj(imageId)).Result.Message;

                if (response.IsSuccess)
                {
                    return _imageInfoMapper.Map(response.Body);
                }

                _logger.LogWarning(logMessage, imageId);
            }
            catch (Exception exc)
            {
                _logger.LogWarning(exc, logMessage, imageId);
            }

            errors.Add(errorMessage);

            return null;
        }

        public GetCompanyCommand(
            ICompanyRepository repository,
            ILogger<GetCompanyCommand> logger,
            ICompanyInfoMapper mapper,
            IImageInfoMapper imageInfoMapper,
            IRequestClient<IGetImageRequest> requestClient)
        {
            _repository = repository;
            _logger = logger;
            _companyInfoMapper = mapper;
            _imageInfoMapper = imageInfoMapper;
            _rcGetFile = requestClient;
        }

        public CompanyResponse Execute()
        {
            DbCompany company = _repository.Get(true);

            List<string> errors = new();
            ImageInfo image = null;
            if (company.LogoId.HasValue)
            {
                image = GetImage(company.LogoId.Value, errors);
            }

            return new CompanyResponse
            {
                Company = _companyInfoMapper.Map(company, image),
                Errors = errors
            };
        }
    }
}
