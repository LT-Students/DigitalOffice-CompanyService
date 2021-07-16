using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Responses.File;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
    public class ImageInfoMapper : IImageInfoMapper
    {
        public ImageInfo Map(IGetImageResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            return new ImageInfo
            {
                Id = response.ImageId,
                ParentId = response.ParentId,
                Content = response.Content,
                Extension = response.Extension,
                Name = response.Name
            };
        }
    }
}
