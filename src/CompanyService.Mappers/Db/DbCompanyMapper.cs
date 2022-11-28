using System;
using System.Threading.Tasks;
using DigitalOffice.Kernel.ImageSupport.Helpers.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.CompanyService.Mappers.Db
{
  public class DbCompanyMapper : IDbCompanyMapper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IImageResizeHelper _imageResizeHelper;

    public DbCompanyMapper(
      IHttpContextAccessor httpContextAccessor,
      IImageResizeHelper imageResizeHelper)
    {
      _httpContextAccessor = httpContextAccessor;
      _imageResizeHelper = imageResizeHelper;
    }

    public async Task<DbCompany> MapAsync(CreateCompanyRequest request)
    {
      if (request is null)
      {
        return null;
      }

      (bool isSucces, string content, string extension) resizeResults = request.Logo is null
        ? default
        : await _imageResizeHelper.ResizeAsync(request.Logo.Content, request.Logo.Extension);

      return new DbCompany
      {
        Id = Guid.NewGuid(),
        Name = request.Name,
        Description = request.Description,
        Tagline = request.Tagline,
        Contacts = request.Contacts,
        CreatedAtUtc = DateTime.UtcNow,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        IsActive = true,
        LogoContent = resizeResults.isSucces ? resizeResults.content : null,
        LogoExtension = resizeResults.isSucces ? resizeResults.extension : null
      };
    }
  }
}
