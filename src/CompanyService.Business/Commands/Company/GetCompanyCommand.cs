using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Requests.File;
using LT.DigitalOffice.Models.Broker.Responses.File;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company
{
  public class GetCompanyCommand : IGetCompanyCommand
  {
    private readonly ICompanyRepository _repository;
    private readonly ICompanyInfoMapper _companyInfoMapper;
   // private readonly IRequestClient<IGetImagesRequest> _requestClient;
    private readonly ILogger<GetCompanyCommand> _logger;
    private readonly IImageInfoMapper _imageMapper;

    private ImageInfo GetImage(Guid? imageId, List<string> errors)
    {
      if (imageId == null)
      {
        return null;
      }

      ImageInfo result = new();

      string errorMessage = $"Can not get image '{imageId}' information. Please try again later.";
      string logMessage = $"Can not get image '{imageId}' information.";

      /*try
      {
        var response = _requestClient.GetResponse<IOperationResult<IGetImagesResponse>>(
            IGetImagesRequest.CreateObj(new() { imageId.Value })).Result.Message;

        if (response.IsSuccess)
        {
          result = _imageMapper.Map(response.Body.Images[0]);
        }
        else
        {
          _logger.LogWarning(logMessage, string.Join('\n', response.Errors));

          errors.Add(errorMessage);
        }
      }
      catch (Exception exc)
      {
        _logger.LogError(exc, errorMessage);

        errors.Add(errorMessage);
      }*/

      return result;
    }

    public GetCompanyCommand(
      ICompanyRepository repository,
      ICompanyInfoMapper mapper,
    //  IRequestClient<IGetImagesRequest> requestClient,
      ILogger<GetCompanyCommand> logger,
      IImageInfoMapper imageMapper)
    {
      _repository = repository;
      _companyInfoMapper = mapper;
     // _requestClient = requestClient;
      _logger = logger;
      _imageMapper = imageMapper;
    }

    public OperationResultResponse<CompanyInfo> Execute(GetCompanyFilter filter)
    {
      List<string> errors = new();
      DbCompany company = _repository.Get(filter);

      return new OperationResultResponse<CompanyInfo>
      {
        Status = errors.Any() ? OperationResultStatusType.PartialSuccess : OperationResultStatusType.FullSuccess,
        Body = _companyInfoMapper.Map(company, GetImage(company?.LogoId, errors), filter)
      };
    }
  }
}
